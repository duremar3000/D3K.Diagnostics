using System;
using System.Collections.Generic;
using System.Linq;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogBehavior<TLogListenerFactory>(this ServiceRegistry serviceRegistry, string name, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListenerFactory>().Use<TLogListenerFactory>().Named(name);
            serviceRegistry.For<ILogListener>().Use(sc => sc.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName)).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>("Target").IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<LogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");            
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterHashCodeMethodLogBehavior<TLogListenerFactory>(this ServiceRegistry serviceRegistry, string name, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListenerFactory>().Use<TLogListenerFactory>().Named(name);
            serviceRegistry.For<ILogListener>().Use(sc => sc.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName)).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}HashCodeMethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<HashCodeLogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterMethodLogBehavior(this ServiceRegistry serviceRegistry, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListener>().Use(logListener).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<LogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterHashCodeMethodLogBehavior(this ServiceRegistry serviceRegistry, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListener>().Use(logListener).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}HashCodeMethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<HashCodeLogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterMethodLogAsyncBehavior<TLogListenerFactory>(this ServiceRegistry serviceRegistry, string name, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogAsyncBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListenerFactory>().Use<TLogListenerFactory>().Named(name);
            serviceRegistry.For<ILogListener>().Use(sc => sc.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName)).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<LogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterHashCodeMethodLogAsyncBehavior<TLogListenerFactory>(this ServiceRegistry serviceRegistry, string name, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogAsyncBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListenerFactory>().Use<TLogListenerFactory>().Named(name);
            serviceRegistry.For<ILogListener>().Use(sc => sc.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName)).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}HashCodeMethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<HashCodeLogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterMethodLogAsyncBehavior(this ServiceRegistry serviceRegistry, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogAsyncBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListener>().Use(logListener).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<LogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        public static void RegisterHashCodeMethodLogAsyncBehavior(this ServiceRegistry serviceRegistry, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodLogAsyncBehavior>().Singleton().Named(name)
                .Ctor<ILogger>().IsNamedInstance(name)
                .Ctor<IMethodLogMessageFactory>().IsNamedInstance(name);

            serviceRegistry.For<ILogger>().Add(sc => CreateLogger(sc.GetInstance<ILogListener>(name))).Named(name);
            serviceRegistry.For<ILogListener>().Use(logListener).Singleton().Named(name);
            serviceRegistry.For<IMethodLogMessageFactory>().Use<ElapsedMethodLogMessageFactory>().Named(name).Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}HashCodeMethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").Setter<IMethodLogMessageFactory>().IsNamedInstance($"{name}MethodLogMessageFactory");
            serviceRegistry.For<IMethodLogMessageFactory>().Use<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IArgListObjectMapper>().IsNamedInstance(name).Ctor<ITypeShortNameFactory>().IsNamedInstance(name).Ctor<ILogMessageFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogMessageSettings>().Use<HashCodeLogMessageSettings>().Named(name);
            serviceRegistry.For<IArgListObjectMapper>().Use<ArgListObjectMapper>().Named(name).Ctor<ILogMessageSettings>().IsNamedInstance(name).Ctor<IDynamicArgListObjectFactory>().IsNamedInstance(name);
            serviceRegistry.For<ILogValueMapper>().Use<LogValueMapper>().Named(name).Ctor<ILogValueMapperConfigurator>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectFactory>().Use<DynamicArgListObjectFactory>().Named(name).Ctor<IDynamicArgListObjectTypeFactory>().IsNamedInstance(name);
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<CachingDynamicArgListObjectTypeFactory>().Named(name).Setter<IDynamicArgListObjectTypeFactory>("Target").IsNamedInstance($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<IDynamicArgListObjectTypeFactory>().Use<DynamicArgListObjectTypeFactory>().Named($"{name}DynamicArgListObjectTypeFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<CachingTypeShortNameFactory>().Named(name).Setter<ITypeShortNameFactory>("Target").IsNamedInstance($"{name}TypeShortNameFactory");
            serviceRegistry.For<ITypeShortNameFactory>().Use<TypeShortNameFactory>().Named($"{name}TypeShortNameFactory");
            serviceRegistry.For<ILogMessageFactory>().Use<LogMessageFactory>().Named(name);
            serviceRegistry.For<ILogValueMapperConfigurator>().Use<DefaultLogValueMapperConfigurator>().Named(name);
        }

        private static ILogger CreateLogger(ILogListener logListener)
        {
            var logger = new Logger();

            logger.Attach(logListener);

            return logger;
        }
    }
}