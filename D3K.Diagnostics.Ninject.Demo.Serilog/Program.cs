using System;
using System.Threading.Tasks;
using System.Threading;

using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Infrastructure.Language;

using D3K.Diagnostics.SerilogExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Ninject.Demo.Serilog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var kernel = new StandardKernel())
            {
                RegisterDependecies(kernel);

                var demoApp = kernel.Get<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(IKernel kernel)
        {
            kernel.RegisterMethodIdentityInterceptor<SerilogLogContext>("pid", "pid");
            kernel.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("log", "Debug");
            //kernel.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("log", "Debug");

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

            var printerBinding = kernel.Bind<IPrinter>().To<Printer>();
            printerBinding.Intercept().With(pid);
            printerBinding.Intercept().With(log);
        }
    }
}
