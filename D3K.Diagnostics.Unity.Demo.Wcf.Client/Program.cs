using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Injection;

using D3K.Diagnostics.Unity.Log;
using D3K.Diagnostics.Unity.Demo.Wcf.Client.ServiceReference;

namespace D3K.Diagnostics.Unity.Demo.Wcf.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string loggerName = "logger";

            var ii = new Interceptor<InterfaceInterceptor>();
            var lb = new InterceptionBehavior<MethodInvokeLoggingBehavior>(loggerName);
            var ib = new InterceptionBehavior<MethodIdentificationBehavior>();

            var container = new UnityContainer()
                .AddNewExtension<Interception>()

                .RegisterMethodInvokeLoggingBehavior(loggerName, "debug")
                .RegisterMethodIdentificationBehavior("mid")

                .RegisterType<IService, ServiceClient>(new InjectionConstructor(), ii, ib, lb);

            var service = container.Resolve<IService>();

            var res = service.DoWork(new DoWorkArgs { Arg1 = "Hello", Arg2 = "World" });

            Console.WriteLine(res.Value);

            Console.ReadLine();

        }
    }
}
