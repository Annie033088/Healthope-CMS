using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;

namespace ApiLayer.App_Start
{
    public class AutofacConfig
    {
        public static void RegisterDependencies()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //註冊 service 跟 repository
            //builder.RegisterType<LoginService>().As<ILoginService>().InstancePerRequest();
            //builder.RegisterType<TaskService>().As<ITaskService>().InstancePerRequest();
            //builder.RegisterType<SessionService>().As<ISessionService>().InstancePerRequest();
            //builder.RegisterType<TaskRepository>().As<ITaskRepository>().InstancePerRequest();
            //builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();

            // 註冊 Mapper
            MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>(); // 註冊你的 AutoMapperProfile
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            // 註冊 Web API 控制器
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            IContainer container = builder.Build();

            // 設定 Autofac 為 Web API 的 DI 容器
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}