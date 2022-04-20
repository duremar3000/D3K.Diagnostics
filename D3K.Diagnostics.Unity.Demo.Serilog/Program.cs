using System;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

using D3K.Diagnostics.SerilogExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Unity.Demo.Serilog
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
                
                .RegisterMethodIdentityInterceptionBehavior<SerilogLogContext>("pid", "pid")
                .RegisterMethodLogInterceptionBehavior<XmlSerilogLogListenerFactory>("log", "Debug")
                //.RegisterMethodLogInterceptionBehavior<JsonSerilogLogListenerFactory>("log", "Debug")

                .RegisterType<IDemoApp, DemoApp>(ii, pid, log)
                .RegisterType<IHelloWorldService, HelloWorldService>(ii, pid, log)
                .RegisterType<IHelloService, HelloService>(ii, pid, log)
                .RegisterType<IWorldService, WorldService>(ii, pid, log)
                .RegisterType<IPrinter, Printer>(ii, pid, log)
                ;
        }
    }
}
