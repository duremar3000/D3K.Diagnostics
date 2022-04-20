using System;
using System.Threading;
using System.Threading.Tasks;

using SimpleInjector;
using SimpleInjector.InterceptorExtensions;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new Container())
            {
                container.Options.AllowOverridingRegistrations = true;

                RegisterDependencies(container);

                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }            
        }

        private static void RegisterDependencies(Container container)
        {
            container.RegisterMethodIdentityAsyncInterceptor<SerilogLogContext>("pid");
            container.RegisterMethodLogAsyncInterceptor<XmlSerilogLogListenerFactory>("Debug");
            //container.RegisterMethodLogAsyncInterceptor<JsonSerilogLogListenerFactory>("Debug");

            container.Register<IDemoApp, DemoApp>();
            container.Register<IHelloWorldService, HelloWorldService>();
            container.Register<IHelloService, HelloService>();
            container.Register<IWorldService, WorldService>();

            container.InterceptWith<MethodLogAsyncInterceptor>(InterceptPredicate);
            container.InterceptWith<MethodIdentityAsyncInterceptor>(InterceptPredicate);


            container.RegisterMethodIdentityInterceptor<SerilogLogContext>("syncPid");
            container.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("Debug");
            //container.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("Debug");

            container.Register<IPrinter, Printer>();

            container.InterceptWith<MethodLogInterceptor>(SyncInterceptPredicate);
            container.InterceptWith<MethodIdentityInterceptor>(SyncInterceptPredicate);
        }

        private static bool InterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo" && type != typeof(IPrinter);
        }

        private static bool SyncInterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo" && type == typeof(IPrinter);
        }
    }
}
