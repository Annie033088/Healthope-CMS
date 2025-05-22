using System;
using System.Web;
using System.Web.Http.Controllers;
using ApiLayer.Interface;

namespace ApiLayer.Service
{
    public class HttpService : IHttpService
    {

        public string GetControllerName(HttpActionContext actionContext)
        {
            try
            {
                return actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetActionName(HttpActionContext actionContext)
        {
            try
            {
                return actionContext.ActionDescriptor.ActionName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetRootPath()
        {
            try
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}