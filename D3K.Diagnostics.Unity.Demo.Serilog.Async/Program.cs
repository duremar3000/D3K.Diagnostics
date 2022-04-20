using System;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

using D3K.Diagnostics.SerilogExtensions;
using D3K.Diagnostics.Unity;
using Serilog;

namespace D3K.Diagnostics.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new UnityContainer())
            {
                RegisterDependencies(container);

                var demoApp = container.Resolve<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependencies(IUnityContainer container)
        {
            var ii = new Interceptor<InterfaceInterceptor>();

            var log = new InterceptionBehavior<MethodLogAsyncInterceptionBehavior>("log");
            var pid = new InterceptionBehavior<MethodIdentityAsyncInterceptionBehavior>("pid");

            var syncLog = new InterceptionBehavior<MethodLogInterceptionBehavior>("syncLog");
            var syncPid = new InterceptionBehavior<MethodIdentityInterceptionBehavior>("syncPid");

            container
                .AddNewExtension<Interception>()

                .RegisterMethodIdentityAsyncInterceptionBehavior<SerilogLogContext>("pid", "pid")
                //.RegisterMethodLogAsyncInterceptionBehavior<JsonSerilogLogListenerFactory>("log", "Debug")
                
                .RegisterMethodIdentityInterceptionBehavior<SerilogLogContext>("syncPid", "pid")
                //.RegisterMethodLogInterceptionBehavior<JsonSerilogLogListenerFactory>("syncLog", "Debug")

                .RegisterType<IDemoApp, DemoApp>(ii, pid, log)
                .RegisterType<IHelloWorldService, HelloWorldService>(ii, pid, log)
                .RegisterType<IHelloService, HelloService>(ii, pid, log)
                .RegisterType<IWorldService, WorldService>(ii, pid, log)
                .RegisterType<IPrinter, Printer>(ii, syncPid, syncLog)
                ;
        }
    }
}
