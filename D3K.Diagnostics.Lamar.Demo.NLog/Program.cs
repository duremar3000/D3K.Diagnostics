using System;
using System.Threading.Tasks;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.NLogExtensions;

using Lamar;
using Lamar.DynamicInterception;

namespace D3K.Diagnostics.Lamar.Demo.NLog
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new Container(RegisterDependecies))
            {
                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(ServiceRegistry serviceRegistry)
        {
            using (var container = new Container(sr =>
            {
                sr.RegisterMethodLogBehavior<NLogLogListenerFactory>("log", "Debug");
                sr.RegisterMethodIdentityBehavior<NLogLogContext>("pid", "pid");
            }))
            {
                var log = container.GetInstance<IInterceptionBehavior>("log");
                var pid = container.GetInstance<IInterceptionBehavior>("pid");

                serviceRegistry.For<IDemoApp>().InterceptWith<IDemoApp, DemoApp>(new[] { pid, log });
                serviceRegistry.For<IHelloWorldService>().InterceptWith<IHelloWorldService, HelloWorldService>(new[] { pid, log });
                serviceRegistry.For<IHelloService>().InterceptWith<IHelloService, HelloService>(new[] { pid, log });
                serviceRegistry.For<IWorldService>().InterceptWith<IWorldService, WorldService>(new[] { pid, log });
                serviceRegistry.For<IPrinter>().InterceptWith<IPrinter, Printer>(new[] { pid, log });
            }
        }
    }
}
