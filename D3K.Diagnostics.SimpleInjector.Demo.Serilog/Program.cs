﻿using System;
using System.Threading;
using System.Threading.Tasks;

using SimpleInjector;
using SimpleInjector.InterceptorExtensions;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Demo;
using D3K.Diagnostics.SerilogExtensions;

namespace D3K.Diagnostics.SimpleInjector.Demo.Serilog
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
            container.RegisterMethodIdentityInterceptor<SerilogLogContext>("pid");
            container.RegisterMethodLogInterceptor<XmlSerilogLogListenerFactory>("Debug");
            //container.RegisterMethodLogInterceptor<JsonSerilogLogListenerFactory>("Debug");

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
