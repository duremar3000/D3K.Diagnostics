using System;

using Castle.Windsor;
using Castle.MicroKernel.Registration;

using D3K.Diagnostics.SerilogExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Windsor.Demo.Serilog.Async
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
            container.RegisterMethodIdentityAsyncInterceptor<SerilogLogContext>("pid", "pid");
            container.RegisterMethodLogAsyncInterceptor<XmlSerilogLogListenerFactory>("log", "Debug");
            //container.RegisterMethodLogAsyncInterceptor<JsonSerilogLogListenerFactory>("log", "Debug");

            container.RegisterMethodIdentityInterceptor<SerilogLogContext>("syncPid", "pid");
            container.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("syncLog", "Debug");
            //container.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("syncLog", "Debug");

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
