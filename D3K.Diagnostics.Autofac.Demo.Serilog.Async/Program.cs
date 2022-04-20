using System;

using Autofac;
using Autofac.Extras.DynamicProxy;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

namespace D3K.Diagnostics.Autofac.Demo.Serilog.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            RegisterDependecies(builder);

            using (var container = builder.Build())
            {
                var demoApp = container.Resolve<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(ContainerBuilder builder)
        {
            builder.RegisterMethodIdentityAsyncInterceptor<SerilogLogContext>("pid", "pid");
            builder.RegisterMethodLogAsyncInterceptor<XmlSerilogLogListenerFactory>("log", "Debug");
            //builder.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("log", "Debug");

            builder.RegisterMethodIdentityInterceptor<SerilogLogContext>("syncPid", "pid");
            builder.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("syncLog", "Debug");
            //builder.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("syncLog", "Debug");

            builder.RegisterType<DemoApp>().As<IDemoApp>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloWorldService>().As<IHelloWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloService>().As<IHelloService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<WorldService>().As<IWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<Printer>().As<IPrinter>().EnableInterfaceInterceptors().InterceptedBy("syncPid").InterceptedBy("syncLog");
        }
    }
}
