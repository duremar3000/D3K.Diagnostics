﻿using System;
using System.Collections.Generic;
using System.Linq;

using SimpleInjector;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SimpleInjector
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, LogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateHashCodeMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, LogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateHashCodeMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogAsyncInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, LogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogAsyncInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateHashCodeMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogAsyncInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogAsyncInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, LogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.RegisterSingleton<MethodLogAsyncInterceptor>();
            container.RegisterSingleton<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.RegisterSingleton(() => CreateHashCodeMethodLogMessageFactory(container));
            container.RegisterSingleton<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.RegisterSingleton<IArgListObjectMapper, ArgListObjectMapper>();
            container.RegisterSingleton<ILogValueMapper, LogValueMapper>();
            container.RegisterSingleton<IDynamicArgListObjectFactory, DynamicArgListObjectFactory>();
            container.RegisterSingleton<IDynamicArgListObjectTypeFactory, DynamicArgListObjectTypeFactory>();
            container.RegisterSingleton<ITypeShortNameFactory, TypeShortNameFactory>();
            container.RegisterSingleton<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.RegisterSingleton<ILogMessageFactory, LogMessageFactory>();
        }

        private static void AttachLogListener(Logger logger, ILogListener logListener)
        {
            logger.Detach(logListener);
            logger.Attach(logListener);
        }

        private static IMethodLogMessageFactory CreateMethodLogMessageFactory(Container container)
        {
            var methodLogMessageFactory = new MethodLogMessageFactory(container.GetInstance<ILogMessageSettings>(), container.GetInstance<IArgListObjectMapper>(), container.GetInstance<ITypeShortNameFactory>(), container.GetInstance<ILogMessageFactory>());
            var elapsedMethodLogMessageFactory = new ElapsedMethodLogMessageFactory { Target = methodLogMessageFactory };

            return elapsedMethodLogMessageFactory;
        }

        private static IMethodLogMessageFactory CreateHashCodeMethodLogMessageFactory(Container container)
        {
            var methodLogMessageFactory = new MethodLogMessageFactory(container.GetInstance<ILogMessageSettings>(), container.GetInstance<IArgListObjectMapper>(), container.GetInstance<ITypeShortNameFactory>(), container.GetInstance<ILogMessageFactory>());
            var hashCodeMethodLogMessageFactory = new HashCodeMethodLogMessageFactory { Target = methodLogMessageFactory };
            var elapsedMethodLogMessageFactory = new ElapsedMethodLogMessageFactory { Target = hashCodeMethodLogMessageFactory };

            return elapsedMethodLogMessageFactory;
        }
    }
}