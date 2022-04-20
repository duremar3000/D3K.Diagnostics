using System;

using SimpleInjector;
using SimpleInjector.InterceptorExtensions;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.SimpleInjector.Demo.Log4net
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new Container())
            {
                RegisterDependencies(container);

                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependencies(Container container)
        {
            container.RegisterMethodIdentityInterceptor<Log4netLogContext>("pid");
            container.RegisterMethodLogInterceptor<Log4netLogListenerFactory>("Debug");

            container.Register<IDemoApp, DemoApp>();
            container.Register<IHelloWorldService, HelloWorldService>();
            container.Register<IHelloService, HelloService>();
            container.Register<IWorldService, WorldService>();
            container.Register<IPrinter, Printer>();

            container.InterceptWith<MethodLogInterceptor>(InterceptPredicate);
            container.InterceptWith<MethodIdentityInterceptor>(InterceptPredicate);
        }

        private static bool InterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo";
        }
    }
}
