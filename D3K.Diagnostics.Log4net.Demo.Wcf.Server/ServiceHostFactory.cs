using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Injection;
using Unity.Wcf;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Core.Log;

namespace D3K.Diagnostics.Log4net.Demo.Wcf.Server
{
    public class ServiceHostFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<ILogger, Logger>(new InjectionMethod(nameof(Logger.Attach), typeof(ILog)))
                .RegisterType<ILog, ObserverTraceLog>(new InjectionConstructor("debug"))

                .RegisterType<IService, LoggingService>(new InjectionConstructor(new ResolvedParameter<IService>("service"), typeof(ILogger)))
                .RegisterType<IService, Service>("service");
        }
    }
}