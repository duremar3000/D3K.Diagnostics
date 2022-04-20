using System;
using System.Threading.Tasks;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Lamar.Demo.Log4net.Async
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
                sr.RegisterMethodLogAsyncBehavior<Log4netLogListenerFactory>("log", "Debug");
                sr.RegisterMethodIdentityAsyncBehavior<Log4netLogContext>("pid", "pid");

                sr.RegisterMethodLogBehavior<Log4netLogListenerFactory>("syncLog", "Debug");
                sr.RegisterMethodIdentityBehavior<Log4netLogContext>("syncPid", "pid");
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
