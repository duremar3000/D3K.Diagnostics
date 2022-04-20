using System;
using System.Threading.Tasks;
using System.Threading;

using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;
using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

namespace D3K.Diagnostics.Ninject.Demo.Serilog.Async
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
            kernel.RegisterMethodIdentityAsyncInterceptor<SerilogLogContext>("pid", "pid");
            kernel.RegisterMethodLogAsyncInterceptor<XmlSerilogLogListenerFactory>("log", "Debug");
            //kernel.RegisterMethodLogAsyncInterceptor<JsonSerilogLogListenerFactory>("log", "Debug");

            kernel.RegisterMethodIdentityInterceptor<SerilogLogContext>("syncPid", "pid");
            kernel.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("syncLog", "Debug");
            //kernel.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("syncLog", "Debug");

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
