using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using ApiLayer.Interface;

namespace ApiLayer.Service
{
    public class HttpHelpter : IHttpHelper
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
    }
}