using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ApiLayer.Interface;
using ApiLayer.Models;
using ApiLayer.Models.Admin;
using DomainLayer.Models;
using DomainLayer.Utility;
using NLog;

namespace ApiLayer.Filters
{
    public class AdminPermissionAuthFilter : ActionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ISessionService sessionService { get; set; }
        public IHttpHelper httpHelper { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                // 通過 controller 跟 action 取得此 action 需要的權限
                string controllerName = httpHelper.GetControllerName(actionContext);
                string actionName = httpHelper.GetActionName(actionContext);

                ActionRequiredPermissionTable actionRequiredPermissionTable = new ActionRequiredPermissionTable();

                // 如果有需要的權限對照表, 就檢查權限
                if (actionRequiredPermissionTable.actionRequiredPermission.TryGetValue(controllerName + "," + actionName, out List<AdminPermission> requiredPermissions))
                {
                    // 取得管理者 session
                    string adminSessionKey = "AdminSession";
                    AdminSession adminSession = sessionService.GetSession<AdminSession>(adminSessionKey);
                    ResultResponse response;
                    bool hasPermission = false;

                    // 使用者是否登入
                    if (adminSession == null)
                    {
                        // 使用者未登入
                        response = new ResultResponse() { ErrorCode = ErrorCodeDefine.UserNotLogin };
                        actionContext.Response = actionContext.Request.CreateResponse(response);
                        return;
                    }

                    AdminPermissionUtility adminPermissionUtility = new AdminPermissionUtility();

                    // 檢查是否有權限
                    foreach (AdminPermission requiredPermission in requiredPermissions)
                    {
                        // 只要有任一權限，則通過
                        if (adminPermissionUtility.HasPermission(adminSession.Identity, requiredPermission))
                        {
                            hasPermission = true;
                        }
                    }

                    // 沒權限則回傳無權限
                    if (!hasPermission)
                    {
                        response = new ResultResponse() { ErrorCode = ErrorCodeDefine.NoPermission };
                        actionContext.Response = actionContext.Request.CreateResponse(response);
                        return;
                    }
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