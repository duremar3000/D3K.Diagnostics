using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Injection;
using Unity.Wcf;

using D3K.Diagnostics.Unity.Log;

namespace D3K.Diagnostics.Wcf.Demo.Server
{
    public class ServiceHostFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            string loggerName = "logger";

            var ii = new Interceptor<InterfaceInterceptor>();
            var lb = new InterceptionBehavior<MethodInvokeLoggingBehavior>(loggerName);
            var ib = new InterceptionBehavior<MethodIdentificationBehavior>();

            container
                .AddNewExtension<Interception>()

                .RegisterMethodInvokeLoggingBehavior(loggerName, "debug")
                .RegisterMethodIdentificationBehavior("mid")

                .RegisterType<IService, Service>(new InjectionConstructor(), ii, ib, lb);
        }
    }
}