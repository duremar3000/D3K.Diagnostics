﻿using System;
using System.Threading.Tasks;

using Serilog;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.SerilogExtensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Lamar.Demo.Serilog
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

                    demoApp.Run();

                    Console.ReadLine();
                }
            }
        }

        public static IHost CreateHost(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)

                .ConfigureLogging((hostBuilderContext, logging) => { logging.AddSerilog(dispose:true); })
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
                sr.RegisterMethodLogBehavior<SerilogLogListenerFactory>("log", "Debug");
                sr.RegisterMethodIdentityBehavior<SerilogLogContext>("pid", "pid");
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
