using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ApiLayer.Models;
using NLog;

namespace ApiLayer.Filters
{
    public class RequestLoggerFilter : ActionFilterAttribute
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                string controllerName = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
                string actionName = actionContext.ActionDescriptor.ActionName;
                logger.Trace($"Executing Controller: {controllerName}, Action: {actionName}");
            }
            catch (Exception ex)
            {
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                actionContext.Response = actionContext.Request.CreateResponse(response);
                logger.Error(ex);
                return;
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                string controllerName = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
                string actionName = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
                logger.Trace($"Executed Controller: {controllerName}, Action: {actionName}");
            }
            catch (Exception ex)
            {
                ResultResponse response = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(response);
                logger.Error(ex);
                return;
            }
        }
    }
}