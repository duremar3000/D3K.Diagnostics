using System;
using System.Threading.Tasks;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.NLogExtensions;

using Lamar;
using Lamar.DynamicInterception;

namespace D3K.Diagnostics.Lamar.Demo.NLog.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new Container(RegisterDependecies))
            {
                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(ServiceRegistry serviceRegistry)
        {
            using (var container = new Container(sr =>
            {
                sr.RegisterMethodLogAsyncBehavior<NLogLogListenerFactory>("log", "Debug");
                sr.RegisterMethodIdentityAsyncBehavior<NLogLogContext>("pid", "pid");

                sr.RegisterMethodLogBehavior<NLogLogListenerFactory>("syncLog", "Debug");
                sr.RegisterMethodIdentityBehavior<NLogLogContext>("syncPid", "pid");
            }))
            {
                var log = container.GetInstance<IInterceptionBehavior>("log");
                var pid = container.GetInstance<IInterceptionBehavior>("pid");

                var syncLog = container.GetInstance<IInterceptionBehavior>("syncLog");
                var syncPid = container.GetInstance<IInterceptionBehavior>("syncPid");

                serviceRegistry.For<IDemoApp>().InterceptWith<IDemoApp, DemoApp>(new[] { pid, log });
                serviceRegistry.For<IHelloWorldService>().InterceptWith<IHelloWorldService, HelloWorldService>(new[] { pid, log });
                serviceRegistry.For<IHelloService>().InterceptWith<IHelloService, HelloService>(new[] { pid, log });
                serviceRegistry.For<IWorldService>().InterceptWith<IWorldService, WorldService>(new[] { pid, log });
                serviceRegistry.For<IPrinter>().InterceptWith<IPrinter, Printer>(new[] { syncPid, syncLog });
            }
        }
    }
}
