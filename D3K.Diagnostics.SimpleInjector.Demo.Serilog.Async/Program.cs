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

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog.Async
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
            container.RegisterMethodIdentityAsyncInterceptor<SerilogLogContext>("pid");
            container.RegisterMethodLogAsyncInterceptor<SerilogLogListenerFactory>("Debug");

            container.Register<IDemoApp, DemoApp>();
            container.Register<IHelloWorldService, HelloWorldService>();
            container.Register<IHelloService, HelloService>();
            container.Register<IWorldService, WorldService>();

            container.InterceptWith<MethodLogAsyncInterceptor>(InterceptPredicate);
            container.InterceptWith<MethodIdentityAsyncInterceptor>(InterceptPredicate);
        }

        private static bool InterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.SimpleInjector.Demo.Serilog.Async";
        }
    }

    public class DemoHostedService : IHostedService
    {
        readonly Container _container;

        public DemoHostedService(Container container)
        {
            _container = container;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var demoApp = _container.GetInstance<IDemoApp>();

            await demoApp.RunAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
