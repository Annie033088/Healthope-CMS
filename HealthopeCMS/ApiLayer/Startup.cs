using System.Configuration;
using ApiLayer.App_Start;
using Autofac;
using Hangfire;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ApiLayer.Startup))]

namespace ApiLayer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 1. 建 Autofac container
            IContainer container = AutofacConfig.RegisterDependencies();

            // 2. 設定 Hangfire 並使用 Autofac 作為 DI 容器
            GlobalConfiguration.Configuration
                .UseAutofacActivator(container)
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString);

            // 3. 啟用 Hangfire server + dashboard
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
