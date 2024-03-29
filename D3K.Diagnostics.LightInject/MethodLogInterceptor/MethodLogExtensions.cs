﻿using System;
using System.Collections.Generic;
using System.Linq;

using LightInject;
using LightInject.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.LightInject
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogInterceptor<TLogListenerFactory>(this IServiceContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<IInterceptor, MethodLogInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.Register<ILogListenerFactory, TLogListenerFactory>(name);
            container.RegisterSingleton(sf => sf.GetInstance<ILogListenerFactory>(name).CreateLogListener(loggerName), name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, LogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this IServiceContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<IInterceptor, MethodLogInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.Register<ILogListenerFactory, TLogListenerFactory>(name);
            container.RegisterSingleton(sf => sf.GetInstance<ILogListenerFactory>(name).CreateLogListener(loggerName), name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.Register<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory");
            container.RegisterPropertyDependency((sf, pi) => pi.DeclaringType == typeof(HashCodeMethodLogMessageFactory) ? sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory") : sf.GetInstance<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogInterceptor(this IServiceContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container.RegisterSingleton<IInterceptor, MethodLogInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.RegisterSingleton(sf => logListener, name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, LogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor(this IServiceContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container.RegisterSingleton<IInterceptor, MethodLogInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.RegisterSingleton(sf => logListener, name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.Register<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory");
            container.RegisterPropertyDependency((sf, pi) => pi.DeclaringType == typeof(HashCodeMethodLogMessageFactory) ? sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory") : sf.GetInstance<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this IServiceContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<IInterceptor, MethodLogAsyncInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.Register<ILogListenerFactory, TLogListenerFactory>(name);
            container.RegisterSingleton(sf => sf.GetInstance<ILogListenerFactory>(name).CreateLogListener(loggerName), name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, LogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this IServiceContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<IInterceptor, MethodLogAsyncInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.Register<ILogListenerFactory, TLogListenerFactory>(name);
            container.RegisterSingleton(sf => sf.GetInstance<ILogListenerFactory>(name).CreateLogListener(loggerName), name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.Register<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory");
            container.RegisterPropertyDependency((sf, pi) => pi.DeclaringType == typeof(HashCodeMethodLogMessageFactory) ? sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory") : sf.GetInstance<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogAsyncInterceptor(this IServiceContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container.RegisterSingleton<IInterceptor, MethodLogAsyncInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.RegisterSingleton(sf => logListener, name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, LogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this IServiceContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentNullException();

            container.RegisterSingleton<IInterceptor, MethodLogAsyncInterceptor>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogger>(name));
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IMethodLogMessageFactory>(name));

            container.Register<ILogger, Logger>(name);
            container.Initialize(sr => sr.ServiceType == typeof(ILogger) && sr.ServiceName == name, (sf, logger) => ((Logger)logger).Attach(sf.GetInstance<ILogListener>(name)));
            container.RegisterSingleton(sf => logListener, name);
            container.Register<IMethodLogMessageFactory, ElapsedMethodLogMessageFactory>(name);
            container.Register<IMethodLogMessageFactory, HashCodeMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory");
            container.RegisterPropertyDependency((sf, pi) => pi.DeclaringType == typeof(HashCodeMethodLogMessageFactory) ? sf.GetInstance<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory") : sf.GetInstance<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>($"{name}MethodLogMessageFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageSettings>(name));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IArgListObjectMapper>(name));
            container.Register<IArgListObjectMapper, ArgListObjectMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapper>(name));
            container.Register<ILogValueMapper, LogValueMapper>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectFactory>(name));
            container.Register<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>(name));
            container.Register<IDynamicArgListObjectTypeFactory, CachingDynamicArgListObjectTypeFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            container.Register<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>(name));
            container.Register<ITypeShortNameFactory, CachingTypeShortNameFactory>(name);
            container.RegisterPropertyDependency((sf, pi) => sf.GetInstance<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            container.Register<ITypeShortNameFactory, TypeShortNameFactory>($"{name}TypeShortNameFactory");
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogMessageFactory>(name));
            container.Register<ILogMessageFactory, LogMessageFactory>(name);
            container.RegisterConstructorDependency((sf, pi) => sf.GetInstance<ILogValueMapperConfigurator>(name));
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>(name);
        }
    }
}