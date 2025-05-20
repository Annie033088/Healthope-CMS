using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin;
using NLog;

namespace ApiLayer.Filters
{
    public class VeriyLoginFilter : ActionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ISessionService sessionService { get; set; }
        public IRedisService redisService { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                string adminSessionKey = "AdminSession";
                AdminSession adminSession = sessionService.GetSession<AdminSession>(adminSessionKey);

                if (adminSession == null)
                {
                    // 使用者未登入卻輸入登入後的url
                    ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.UserNotLogin };
                    actionContext.Response = actionContext.Request.CreateResponse(response);
                    return;
                }

                //取得 redis 的 admin 資料(包括 sessionId 跟 errorCode)
                string redisKey = "Admin" + adminSession.AdminId;
                AdminRedis adminRedis = redisService.GetValue<AdminRedis>(redisKey);

                // redis 不見了
                if (adminRedis == null)
                {
                    ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.UserNotLogin };
                    actionContext.Response = actionContext.Request.CreateResponse(response);
                    // 清掉 asp.net 會話相關資料
                    sessionService.ClearSerssion();
                    return;
                }

                //判斷現 sessionId 是否相同, 不同的話清除 session
                if (adminRedis.SessionId != sessionService.GetSessionId())
                {
                    //後踢前的 errorCode
                    ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.KickOut };
                    actionContext.Response = actionContext.Request.CreateResponse(response);

                    // 清掉 asp.net 跟 redis 的會話相關資料
                    sessionService.ClearSerssion();
                    redisService.DeleteKey(redisKey);
                    return;
                }

                // 判斷是否被禁用
                if (adminRedis.ErrorCode == ErrorCodeDefine.Baned)
                {
                    //禁用的 errorCode
                    ResultResponse response = new ResultResponse() { ErrorCode = adminRedis.ErrorCode };
                    actionContext.Response = actionContext.Request.CreateResponse(response);

                    // 清掉 asp.net 跟 redis 的會話相關資料
                    sessionService.ClearSerssion();
                    redisService.DeleteKey(redisKey);
                    return;
                }

                // 權限是否被更動
                if (adminRedis.ErrorCode == ErrorCodeDefine.PermissionModified)
                {
                    // 權限被更動的 errorCode
                    ResultResponse response = new ResultResponse() { ErrorCode = adminRedis.ErrorCode };
                    actionContext.Response = actionContext.Request.CreateResponse(response);

                    // 清掉 asp.net 跟 redis 的會話相關資料
                    sessionService.ClearSerssion();
                    redisService.DeleteKey(redisKey);
                    return;
                }
            }
            catch (Exception ex)
            {
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                actionContext.Response = actionContext.Request.CreateResponse(response);
                logger.Error(ex);
                return;
            }
        }
    }
}