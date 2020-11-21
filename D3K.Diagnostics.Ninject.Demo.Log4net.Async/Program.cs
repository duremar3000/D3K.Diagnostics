using System;
using System.Threading.Tasks;
using System.Threading;

using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;

using D3K.Diagnostics.Log4netExtensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace D3K.Diagnostics.Ninject.Demo.Log4net.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = CreateHost(args))
            {
                host.RunAsync();

                Console.ReadLine();
            }
        }

        public static IHost CreateHost(string[] args)
        {
            var kernel = new StandardKernel();

            RegisterDependecies(kernel);

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) => { logging.AddLog4Net(); })
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddScoped<IKernel, StandardKernel>(serviceProvider => kernel);
                    services.AddHostedService<DemoHostedService>();
                })
                .UseConsoleLifetime();

            var host = hostBuilder.Build();

            return host;
        }

        private static void RegisterDependecies(IKernel kernel)
        {
            kernel.RegisterMethodIdentityAsyncInterceptor<Log4netLogContext>("pid", "pid");
            kernel.RegisterMethodLogAsyncInterceptor<Log4netLogListenerFactory>("log", "Debug");

            var log = kernel.Get<IInterceptor>("log");
            var pid = kernel.Get<IInterceptor>("pid");

            var demoAppBinding = kernel.Bind<IDemoApp>().To<DemoApp>();
            demoAppBinding.Intercept().With(pid);
            demoAppBinding.Intercept().With(log);

            var helloWorldServiceBinding = kernel.Bind<IHelloWorldService>().To<HelloWorldService>();
            helloWorldServiceBinding.Intercept().With(pid);
            helloWorldServiceBinding.Intercept().With(log);

            var helloServiceBinding = kernel.Bind<IHelloService>().To<HelloService>();
            helloServiceBinding.Intercept().With(pid);
            helloServiceBinding.Intercept().With(log);

            var worldServiceBinding = kernel.Bind<IWorldService>().To<WorldService>();
            worldServiceBinding.Intercept().With(pid);
            worldServiceBinding.Intercept().With(log);
        }
    }

    public class DemoHostedService : IHostedService
    {
        readonly IKernel _kernel;

        public DemoHostedService(IKernel kernel)
        {
            _kernel = kernel;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var demoApp = _kernel.Get<IDemoApp>();

            await demoApp.RunAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
