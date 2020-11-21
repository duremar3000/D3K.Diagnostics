using System;

using Serilog;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Microsoft.DependencyInjection;

using D3K.Diagnostics.SerilogExtensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Unity.Demo.Serilog.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = CreateHostBuilder(args);

            var host = hostBuilder.Build();

            using (var sc = host.Services.CreateScope())
            {
                var demoApp = sc.ServiceProvider.GetRequiredService<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var container = new UnityContainer();

            RegisterDependencies(container);

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddSerilog(dispose: true); })
                .UseUnityServiceProvider(container)
                .UseConsoleLifetime();

            return hostBuilder;
        }

        private static void RegisterDependencies(IUnityContainer container)
        {
            var ii = new Interceptor<InterfaceInterceptor>();

            var log = new InterceptionBehavior<MethodLogAsyncInterceptionBehavior>("log");
            var pid = new InterceptionBehavior<MethodIdentityAsyncInterceptionBehavior>("pid");

            container
                .AddNewExtension<Interception>()

                .RegisterMethodLogAsyncInterceptionBehavior<SerilogLogListenerFactory>("log", "Debug")
                .RegisterMethodIdentityAsyncInterceptionBehavior<SerilogLogContext>("pid", "pid")

                .RegisterType<IDemoApp, DemoApp>(ii, pid, log)
                .RegisterType<IHelloWorldService, HelloWorldService>(ii, pid, log)
                .RegisterType<IHelloService, HelloService>(ii, pid, log)
                .RegisterType<IWorldService, WorldService>(ii, pid, log);
        }
    }
}
