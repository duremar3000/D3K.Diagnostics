using System;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extensions.DependencyInjection;

using D3K.Diagnostics.Log4netExtensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Autofac.Demo.Log4net.Async
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
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(RegisterDependecies)
                .UseConsoleLifetime();

            var host = hostBuilder.Build();

            return host;
        }

        private static void RegisterDependecies(ContainerBuilder builder)
        {
            builder.RegisterMethodIdentityAsyncInterceptor<Log4netLogContext>("pid", "pid");
            builder.RegisterMethodLogAsyncInterceptor<Log4netLogListenerFactory>("log", "Debug");

            builder.RegisterType<DemoApp>().As<IDemoApp>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloWorldService>().As<IHelloWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloService>().As<IHelloService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<WorldService>().As<IWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
        }
    }
}
