﻿using System;

using LightInject;
using LightInject.Interception;

using D3K.Diagnostics.Demo;
using D3K.Diagnostics.NLogExtensions;

namespace D3K.Diagnostics.LightInject.Demo.NLog.Async
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var container = new ServiceContainer())
            {
                RegisterDependecies(container);

                var demoApp = container.GetInstance<IDemoApp>();

                demoApp.RunAsync();

                Console.ReadLine();
            }
        }

        private static void RegisterDependecies(IServiceContainer container)
        {
            container.RegisterMethodLogAsyncInterceptor<NLogLogListenerFactory>("log", "Debug");
            container.RegisterMethodIdentityAsyncInterceptor<NLogLogContext>("pid", "pid");

            container.RegisterMethodLogInterceptor<NLogLogListenerFactory>("syncLog", "Debug");
            container.RegisterMethodIdentityInterceptor<NLogLogContext>("syncPid", "pid");

            container.Intercept(InterceptPredicate, DefineProxyType);
            container.Intercept(SyncInterceptPredicate, SyncDefineProxyType);

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
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo" && type != typeof(IPrinter);
        }

        private static void DefineProxyType(IServiceFactory serviceFactory, ProxyDefinition proxyDefinition)
        {
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("pid"));
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("log"));
        }

        private static bool SyncInterceptPredicate(ServiceRegistration serviceRegistration)
        {
            return SyncInterceptPredicate(serviceRegistration.ServiceType);
        }

        private static bool SyncInterceptPredicate(Type type)
        {
            return type.IsInterface && type.Namespace == "D3K.Diagnostics.Demo" && type == typeof(IPrinter);
        }

        private static void SyncDefineProxyType(IServiceFactory serviceFactory, ProxyDefinition proxyDefinition)
        {
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("syncPid"));
            proxyDefinition.Implement(() => serviceFactory.GetInstance<IInterceptor>("syncLog"));
        }
    }
}
