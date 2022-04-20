using System;

using LightInject;
using LightInject.Interception;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.LightInject.Demo.Log4net
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
            container.RegisterMethodLogInterceptor<Log4netLogListenerFactory>("log", "Debug");
            container.RegisterMethodIdentityInterceptor<Log4netLogContext>("pid", "pid");

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
