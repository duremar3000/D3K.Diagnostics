using System;
using System.Collections.Generic;
using System.Linq;

using Unity;
using Unity.Injection;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Unity
{
    public static class MethodLogBehaviorExtensions
    {
        public static IUnityContainer RegisterMethodLogBehavior<TLogObserver>(this IUnityContainer container, string name, string loggerName) where TLogObserver : ILogObserver
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogBehavior>(name, new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogObserver>(name)))
                .RegisterType<ILogObserver, TLogObserver>(name, new InjectionConstructor(loggerName))
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodLogAsyncBehavior<TLogObserver>(this IUnityContainer container, string name, string loggerName) where TLogObserver : ILogObserver
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogAsyncBehavior>(name, new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogObserver>(name)))
                .RegisterType<ILogObserver, TLogObserver>(name, new InjectionConstructor(loggerName))
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }
    }
}
