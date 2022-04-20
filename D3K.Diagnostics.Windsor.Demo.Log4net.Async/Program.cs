using System;

using Castle.Windsor;
using Castle.MicroKernel.Registration;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Windsor.Demo.Log4net.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new WindsorContainer())
            {
                RegisterDependencies(container);

                var demoApp = container.Resolve<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependencies(IWindsorContainer container)
        {
            container.RegisterMethodIdentityAsyncInterceptor<Log4netLogContext>("pid", "pid");
            container.RegisterMethodLogAsyncInterceptor<Log4netLogListenerFactory>("log", "Debug");

            container.RegisterMethodIdentityInterceptor<Log4netLogContext>("syncPid", "pid");
            container.RegisterMethodLogInterceptor<Log4netLogListenerFactory>("syncLog", "Debug");

            container.Register(
                Component.For<IDemoApp>().ImplementedBy<DemoApp>().Interceptors("pid", "log"),
                Component.For<IHelloWorldService>().ImplementedBy<HelloWorldService>().Interceptors("pid", "log"),
                Component.For<IHelloService>().ImplementedBy<HelloService>().Interceptors("pid", "log"),
                Component.For<IWorldService>().ImplementedBy<WorldService>().Interceptors("pid", "log"),
                Component.For<IPrinter>().ImplementedBy<Printer>().Interceptors("syncPid", "syncLog"))
                ;
        }
    }
}
