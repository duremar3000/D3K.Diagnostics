using System;

using Autofac;
using Autofac.Extras.DynamicProxy;

using D3K.Diagnostics.Log4netExtensions;
using D3K.Diagnostics.Demo;

namespace D3K.Diagnostics.Autofac.Demo.Log4net
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

                demoApp.Run();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(ContainerBuilder builder)
        {
            builder.RegisterMethodIdentityInterceptor<Log4netLogContext>("pid", "pid");
            builder.RegisterMethodLogInterceptor<Log4netLogListenerFactory>("log", "Debug");

            builder.RegisterType<DemoApp>().As<IDemoApp>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloWorldService>().As<IHelloWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<HelloService>().As<IHelloService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<WorldService>().As<IWorldService>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
            builder.RegisterType<Printer>().As<IPrinter>().EnableInterfaceInterceptors().InterceptedBy("pid").InterceptedBy("log");
        }
    }
}
