﻿using System;

using NLog.Extensions.Logging;

using Castle.Windsor.MsDependencyInjection;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using D3K.Diagnostics.NLogExtensions;

namespace D3K.Diagnostics.Windsor.Demo.NLog
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
                .ConfigureLogging((hostingContext, logging) => { logging.AddNLog(); })
                .UseServiceProviderFactory(new WindsorServiceProviderFactory())
                .ConfigureContainer<IWindsorContainer>(RegisterDependencies)
                .UseConsoleLifetime();

            var host = hostBuilder.Build();

            return host;
        }

        private static void RegisterDependencies(IWindsorContainer container)
        {
            container.RegisterMethodIdentityInterceptor<NLogLogContext>("pid", "pid");
            container.RegisterMethodLogInterceptor<NLogLogListenerFactory>("log", "Debug");

            container.Register(
                Component.For<IDemoApp>().ImplementedBy<DemoApp>().Interceptors("pid", "log"),
                Component.For<IHelloWorldService>().ImplementedBy<HelloWorldService>().Interceptors("pid", "log"),
                Component.For<IHelloService>().ImplementedBy<HelloService>().Interceptors("pid", "log"),
                Component.For<IWorldService>().ImplementedBy<WorldService>().Interceptors("pid", "log"));
        }
    }
}
