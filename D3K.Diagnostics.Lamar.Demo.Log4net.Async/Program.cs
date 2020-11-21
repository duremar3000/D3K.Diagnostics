using System;
using System.Threading.Tasks;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.Log4netExtensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Lamar.Demo.Log4net.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = CreateHost(args))
            {
                using (var sc = host.Services.CreateScope())
                {
                    var demoApp = sc.ServiceProvider.GetRequiredService<IDemoApp>();

                    demoApp.RunAsync();

                    Console.ReadLine();
                }
            }
        }

        public static IHost CreateHost(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)

                .ConfigureLogging((hostBuilderContext, logging) => { logging.AddLog4Net(); })
                .UseServiceProviderFactory<ServiceRegistry>(new LamarServiceProviderFactory())
                .ConfigureContainer<ServiceRegistry>(RegisterDependecies)
                .UseConsoleLifetime();

            var host = hostBuilder.Build();

            return host;
        }

        private static void RegisterDependecies(ServiceRegistry serviceRegistry)
        {
            var container = new Container(sr =>
            {
                sr.RegisterMethodLogAsyncBehavior<Log4netLogListenerFactory>("log", "Debug");
                sr.RegisterMethodIdentityAsyncBehavior<Log4netLogContext>("pid", "pid");
            });

            var log = container.GetInstance<IInterceptionBehavior>("log");
            var pid = container.GetInstance<IInterceptionBehavior>("pid");

            serviceRegistry.For<IDemoApp>().InterceptWith<IDemoApp, DemoApp>(new[] { pid, log });
            serviceRegistry.For<IHelloWorldService>().InterceptWith<IHelloWorldService, HelloWorldService>(new[] { pid, log });
            serviceRegistry.For<IHelloService>().InterceptWith<IHelloService, HelloService>(new[] { pid, log });
            serviceRegistry.For<IWorldService>().InterceptWith<IWorldService, WorldService>(new[] { pid, log });
        }
    }
}
