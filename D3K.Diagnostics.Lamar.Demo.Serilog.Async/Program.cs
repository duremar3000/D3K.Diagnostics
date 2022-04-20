using System;
using System.Threading.Tasks;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

namespace D3K.Diagnostics.Lamar.Demo.Serilog.Async
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
                sr.RegisterMethodIdentityAsyncBehavior<SerilogLogContext>("pid", "pid");
                sr.RegisterMethodLogAsyncBehavior<XmlSerilogLogListenerFactory>("log", "Debug");
                //sr.RegisterMethodLogAsyncBehavior<JsonSerilogLogListenerFactory>("log", "Debug");

                sr.RegisterMethodIdentityBehavior<SerilogLogContext>("syncPid", "pid");
                sr.RegisterMethodLogBehavior<XmlSerilogLogListenerFactory>("syncLog", "Debug");
                //sr.RegisterMethodLogBehavior<JsonSerilogLogListenerFactory>("syncLog", "Debug");
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
