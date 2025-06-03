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
using Hangfire;
using Hangfire.SqlServer;
using System.Configuration;

namespace ApiLayer
{
    public class WebApiApplication : HttpApplication
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private BackgroundJobServer hangfireServer;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.RegisterDependencies();
            LogManager.Setup().LoadConfigurationFromFile("NLog.config");
            // Hangfire 使用 SQL Server 儲存工作狀態
            // 啟動 Hangfire Server
            Hangfire.GlobalConfiguration.Configuration
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);
            hangfireServer = new BackgroundJobServer();

            logger.Info("Application Start");
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
        // TODO: 全局錯誤待驗證 
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
            hangfireServer?.Dispose();
            LogManager.Shutdown();
        }
    }
}
