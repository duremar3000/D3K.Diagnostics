using System;
using System.Collections.Generic;
using System.Linq;

using Unity;
using Unity.Injection;
using Unity.Lifetime;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    public static class MethodLogExtensions
    {
        public static IUnityContainer RegisterMethodLogInterceptionBehavior<TLogListenerFactory>(this IUnityContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterType<ILogListenerFactory, TLogListenerFactory>(name)
                .RegisterFactory<ILogListener>(name, c => c.Resolve<ILogListenerFactory>(name).CreateLogListener(loggerName), new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, LogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterHashCodeMethodLogInterceptionBehavior<TLogListenerFactory>(this IUnityContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterType<ILogListenerFactory, TLogListenerFactory>(name)
                .RegisterFactory<ILogListener>(name, c => c.Resolve<ILogListenerFactory>(name).CreateLogListener(loggerName), new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory", new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, HashCodeLogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodLogInterceptionBehavior(this IUnityContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container
                .RegisterType<MethodLogInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterInstance(name, logListener, new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, LogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterHashCodeMethodLogInterceptionBehavior(this IUnityContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container
                .RegisterType<MethodLogInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterInstance(name, logListener, new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory", new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, HashCodeLogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodLogAsyncInterceptionBehavior<TLogListenerFactory>(this IUnityContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogAsyncInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterType<ILogListenerFactory, TLogListenerFactory>(name)
                .RegisterFactory<ILogListener>(name, c => c.Resolve<ILogListenerFactory>(name).CreateLogListener(loggerName), new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, LogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterHashCodeMethodLogAsyncInterceptionBehavior<TLogListenerFactory>(this IUnityContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container
                .RegisterType<MethodLogAsyncInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterType<ILogListenerFactory, TLogListenerFactory>(name)
                .RegisterFactory<ILogListener>(name, c => c.Resolve<ILogListenerFactory>(name).CreateLogListener(loggerName), new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory", new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, HashCodeLogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodLogAsyncInterceptionBehavior(this IUnityContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container
                .RegisterType<MethodLogAsyncInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterInstance(name, logListener, new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, LogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }

        public static IUnityContainer RegisterHashCodeMethodLogAsyncInterceptionBehavior(this IUnityContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container
                .RegisterType<MethodLogAsyncInterceptionBehavior>(name, new ContainerControlledLifetimeManager(), new InjectionConstructor(new ResolvedParameter<ILogger>(name), new ResolvedParameter<IMethodLogMessageFactory>(name)))
                .RegisterType<ILogger, Logger>(name, new InjectionMethod(nameof(Logger.Attach), new ResolvedParameter<ILogListener>(name)))
                .RegisterInstance(name, logListener, new ContainerControlledLifetimeManager())
                .RegisterType<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory", new InjectionProperty("Target", new ResolvedParameter<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")))
                .RegisterType<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory", new InjectionConstructor(new ResolvedParameter<ILogMessageSettings>(name), new ResolvedParameter<IArgListObjectMapper>(name), new ResolvedParameter<ITypeShortNameFactory>(name), new ResolvedParameter<ILogMessageFactory>(name)))
                .RegisterType<ILogMessageSettings, HashCodeLogMessageSettings>(name)
                .RegisterType<IArgListObjectMapper, ArgListObjectMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapper>(name), new ResolvedParameter<IDynamicArgListObjectFactory>(name)))
                .RegisterType<ILogValueMapper, LogValueMapper>(name, new InjectionConstructor(new ResolvedParameter<ILogValueMapperConfigurator>(name)))
                .RegisterType<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name, new InjectionConstructor(new ResolvedParameter<IDynamicArgListObjectTypeFactory>(name)))
                .RegisterType<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name, new InjectionProperty("Target", new ResolvedParameter<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")))
                .RegisterType<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory")
                .RegisterType<ITypeShortNameFactory, CachingTypeShortNameFactory>(name, new InjectionProperty("Target", new ResolvedParameter<ITypeShortNameFactory>($"{name}TypeShortNameFactory")))
                .RegisterType<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory")
                .RegisterType<ILogMessageFactory, LogMessageFactory>(name)
                .RegisterType<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);

            return container;
        }
    }
}