using System;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Unity.Demo.Log4net
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new UnityContainer())
            {
                RegisterDependencies(container);

                var demoApp = container.Resolve<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }            
        }

        private static void RegisterDependencies(IUnityContainer container)
        {
            var ii = new Interceptor<InterfaceInterceptor>();

            var log = new InterceptionBehavior<MethodLogInterceptionBehavior>("log");
            var pid = new InterceptionBehavior<MethodIdentityInterceptionBehavior>("pid");

            container
                .AddNewExtension<Interception>()

                .RegisterMethodLogInterceptionBehavior<Log4netLogListenerFactory>("log", "Debug")
                .RegisterMethodIdentityInterceptionBehavior<Log4netLogContext>("pid", "pid")

                .RegisterType<IDemoApp, DemoApp>(ii, pid, log)
                .RegisterType<IHelloWorldService, HelloWorldService>(ii, pid, log)
                .RegisterType<IHelloService, HelloService>(ii, pid, log)
                .RegisterType<IWorldService, WorldService>(ii, pid, log)
                .RegisterType<IPrinter, Printer>(ii, pid, log)
                ;
        }
    }
}
