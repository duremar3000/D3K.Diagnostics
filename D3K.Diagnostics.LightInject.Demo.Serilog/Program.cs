using System;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

using LightInject;
using LightInject.Interception;

namespace D3K.Diagnostics.LightInject.Demo.Serilog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new ServiceContainer())
            {
                RegisterDependecies(container);

                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(IServiceContainer container)
        {
            container.RegisterMethodIdentityInterceptor<SerilogLogContext>("pid", "pid");
            container.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("log", "Debug");
            //container.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("log", "Debug");

            container.Intercept(InterceptPredicate, DefineProxyType);

            container.Register<IDemoApp, DemoApp>();
            container.Register<IHelloWorldService, HelloWorldService>();
            container.Register<IHelloService, HelloService>();
            container.Register<IWorldService, WorldService>();
            container.Register<IPrinter, Printer>();
        }

        private static bool InterceptPredicate(ServiceRegistration serviceRegistration)
        {
            return InterceptPredicate(serviceRegistration.ServiceType);
        }

        private static bool InterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo";
        }

        private static void DefineProxyType(IServiceFactory serviceFactory, ProxyDefinition proxyDefinition)
        {
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("pid"));
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("log"));
        }
    }
}
