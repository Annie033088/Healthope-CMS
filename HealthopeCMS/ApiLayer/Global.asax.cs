using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ApiLayer.App_Start;
using ApiLayer.Models;
using Newtonsoft.Json;
using NLog;

namespace ApiLayer
{
    public class WebApiApplication : HttpApplication
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.RegisterDependencies();
            LogManager.Setup().LoadConfigurationFromFile("NLog.config");
            logger.Info("Application Start");
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
        // TODO: ¥þ§½¿ù»~«ÝÅçÃÒ 
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            logger.Error(ex);
            ResultResponse errorResponse = new ResultResponse() { ErrorCode = ErrorCodeDefine.ServerError };

            Server.ClearError();

            Response.Clear();
            Response.ContentType = "application/json";
            Response.StatusCode = 200;

            string json = JsonConvert.SerializeObject(errorResponse);
            Response.Write(json);
            Response.End();
        }

        protected void Application_End()
        {
            LogManager.Shutdown();
        }
    }
}
