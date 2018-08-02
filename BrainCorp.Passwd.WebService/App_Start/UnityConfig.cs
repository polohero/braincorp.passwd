using System.Web.Http;
using Unity;
using Unity.WebApi;
using Unity.Injection;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Caching;
using BrainCorp.Passwd.DataAccess;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.WebService.Configuration;

using BrainCorp.Passwd.WebService.Diagnostics;

namespace BrainCorp.Passwd.WebService
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            PasswdConfig config = new PasswdConfig();

            container.RegisterType<ILogger, DiagnosticsLogger>();

            
            container.RegisterType<IConfigValues, PasswdConfig>();
            container.RegisterType<IUserFileConfig, PasswdConfig>();
            container.RegisterType<IGroupFileConfig, PasswdConfig>();
            container.RegisterType<IAWSConfig, PasswdConfig>();
            
            container.RegisterType<ICaching, InfiniteReferenceCaching>();

            if(config.DataAccessSource == DataAccessSourceEnum.FileInS3)
            {
                container.RegisterType<IGroupDataAccess, S3GroupDataAccess>(
                    new InjectionConstructor(
                        typeof(IGroupFileConfig),
                        typeof(ILogger),
                        typeof(IAWSConfig)));
            }
            else
            {
                container.RegisterType<IGroupDataAccess, FileGroupDataAccess>(
                    new InjectionConstructor(typeof(IGroupFileConfig), typeof(ILogger)));
            }

            if (config.DataAccessSource == DataAccessSourceEnum.FileInS3)
            {
                container.RegisterType<IUserDataAccess, S3UserDataAccess>(
                    new InjectionConstructor(
                        typeof(IUserFileConfig), 
                        typeof(ILogger),
                        typeof(IAWSConfig)));
            }
            else
            {
                container.RegisterType<IUserDataAccess, FileUserDataAccess>(
                    new InjectionConstructor(typeof(IUserFileConfig), typeof(ILogger)));
            }

            container.RegisterType<IPasswdProvider, PasswdProvider>(
                new InjectionConstructor(
                    typeof(IUserDataAccess),
                    typeof(IGroupDataAccess),
                    typeof(ICaching),
                    typeof(ILogger),
                    typeof(IConfigValues)));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}