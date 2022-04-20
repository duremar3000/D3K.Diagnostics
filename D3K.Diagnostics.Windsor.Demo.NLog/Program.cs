using System;

using Castle.Windsor;
using Castle.MicroKernel.Registration;

using D3K.Diagnostics.NLogExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Windsor.Demo.NLog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new WindsorContainer())
            {
                RegisterDependencies(container);

                var demoApp = container.Resolve<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependencies(IWindsorContainer container)
        {
            container.RegisterMethodIdentityInterceptor<NLogLogContext>("pid", "pid");
            container.RegisterMethodLogInterceptor<NLogLogListenerFactory>("log", "Debug");

            container.Register(
                Component.For<IDemoApp>().ImplementedBy<DemoApp>().Interceptors("pid", "log"),
                Component.For<IHelloWorldService>().ImplementedBy<HelloWorldService>().Interceptors("pid", "log"),
                Component.For<IHelloService>().ImplementedBy<HelloService>().Interceptors("pid", "log"),
                Component.For<IWorldService>().ImplementedBy<WorldService>().Interceptors("pid", "log"),
                Component.For<IPrinter>().ImplementedBy<Printer>().Interceptors("pid", "log"))
                ;
        }
    }
}
