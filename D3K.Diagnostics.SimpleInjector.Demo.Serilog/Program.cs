using System;
using System.Threading;
using System.Threading.Tasks;

using Serilog;

using SimpleInjector;
using SimpleInjector.InterceptorExtensions;
using SimpleInjector.Lifestyles;

using D3K.Diagnostics.SerilogExtensions;
using D3K.Diagnostics.Castle;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = CreateHost(args))
            {
                host.Run();

                Console.ReadLine();
            }
        }

        public static IHost CreateHost(string[] args)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            RegisterDependencies(container);

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostBuilderContext, logging) => { logging.AddSerilog(dispose: true); })
                .ConfigureServices((hostBuilderContext, services) => { services.AddSimpleInjector(container, options => { options.AddHostedService<DemoHostedService>(); }); })
                .UseConsoleLifetime();

            var host = hostBuilder.Build()
                .UseSimpleInjector(container);

            return host;
        }

        private static void RegisterDependencies(Container container)
        {
            container.RegisterMethodIdentityInterceptor<SerilogLogContext>("pid");
            container.RegisterMethodLogInterceptor<SerilogLogListenerFactory>("Debug");

            container.Register<IDemoApp, DemoApp>();
            container.Register<IHelloWorldService, HelloWorldService>();
            container.Register<IHelloService, HelloService>();
            container.Register<IWorldService, WorldService>();

            container.InterceptWith<MethodLogInterceptor>(InterceptPredicate);
            container.InterceptWith<MethodIdentityInterceptor>(InterceptPredicate);
        }

        private static bool InterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.SimpleInjector.Demo_Serilog";
        }
    }

    public class DemoHostedService : IHostedService
    {
        readonly Container _container;

        public DemoHostedService(Container container)
        {
            _container = container;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var demoApp = _container.GetInstance<IDemoApp>();

            demoApp.Run();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
