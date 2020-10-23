using System;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Microsoft.DependencyInjection;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Unity;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Log4net.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var ii = new Interceptor<InterfaceInterceptor>();

            var lb = new InterceptionBehavior<MethodLogBehavior>("log");
            var pb = new InterceptionBehavior<MethodIdentityBehavior>("log");

            var container = new UnityContainer()
                .AddNewExtension<Interception>()

                .RegisterMethodLogBehavior<Log4netLogObserver>("log", "Debug")
                .RegisterMethodIdentityBehavior<Log4netLoggingLogicalThreadContext>("log", "pid")

                .RegisterType<IDemoApp, DemoApp>(ii, pb, lb)
                .RegisterType<IHelloWorldService, HelloWorldService>(ii, pb, lb)
                .RegisterType<IHelloService, HelloService>(ii, pb, lb)
                .RegisterType<IWorldService, WorldService>(ii, pb, lb);

            var hostBuilder = CreateHostBuilder(args, container);

            var host = hostBuilder.Build();

            using (var sc = host.Services.CreateScope())
            {
                var demoApp = sc.ServiceProvider.GetRequiredService<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IUnityContainer container)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddLog4Net(); })
                .UseUnityServiceProvider(container)
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}
