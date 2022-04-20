using System;
using System.Threading.Tasks;
using System.Threading;

using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Ninject.Demo.Log4net.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var kernel = new StandardKernel())
            {
                RegisterDependecies(kernel);

                var demoApp = kernel.Get<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(IKernel kernel)
        {
            kernel.RegisterMethodIdentityAsyncInterceptor<Log4netLogContext>("pid", "pid");
            kernel.RegisterMethodLogAsyncInterceptor<Log4netLogListenerFactory>("log", "Debug");

            kernel.RegisterMethodIdentityInterceptor<Log4netLogContext>("syncPid", "pid");
            kernel.RegisterMethodLogInterceptor<Log4netLogListenerFactory>("syncLog", "Debug");

            var log = kernel.Get<IInterceptor>("log");
            var pid = kernel.Get<IInterceptor>("pid");

            var syncLog = kernel.Get<IInterceptor>("syncLog");
            var syncPid = kernel.Get<IInterceptor>("syncPid");

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

            var printerBinding = kernel.Bind<IPrinter>().To<Printer>();
            printerBinding.Intercept().With(syncPid);
            printerBinding.Intercept().With(syncLog);
        }
    }
}
