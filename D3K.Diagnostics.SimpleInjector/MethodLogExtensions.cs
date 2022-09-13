using System;
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

            container.Register<MethodLogInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, LogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register<MethodLogInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateHashCodeMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, LogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateHashCodeMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, LogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateHashCodeMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterMethodLogAsyncInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, LogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>(Lifestyle.Singleton);
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => AttachLogListener(logger, container.GetInstance<ILogListener>()));
            container.Register(() => CreateHashCodeMethodLogMessageFactory(container));
            container.Register<ILogMessageSettings, HashCodeLogMessageSettings>();
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
            container.Register<ILogMessageFactory, LogMessageFactory>();
        }

        private static void AttachLogListener(Logger logger, ILogListener logListener)
        {
            logger.Detach(logListener);
            logger.Attach(logListener);
        }

        private static IMethodLogMessageFactory CreateMethodLogMessageFactory(Container container)
        {
            var methodLogMessageFactory = new MethodLogMessageFactory(container.GetInstance<ILogMessageSettings>(), container.GetInstance<ILogValueMapper>(), container.GetInstance<ILogMessageFactory>());
            var elapsedMethodLogMessageFactory = new ElapsedMethodLogMessageFactory { Target = methodLogMessageFactory };

            return elapsedMethodLogMessageFactory;
        }

        private static IMethodLogMessageFactory CreateHashCodeMethodLogMessageFactory(Container container)
        {
            var methodLogMessageFactory = new MethodLogMessageFactory(container.GetInstance<ILogMessageSettings>(), container.GetInstance<ILogValueMapper>(), container.GetInstance<ILogMessageFactory>());
            var hashCodeMethodLogMessageFactory = new HashCodeMethodLogMessageFactory { Target = methodLogMessageFactory };
            var elapsedMethodLogMessageFactory = new ElapsedMethodLogMessageFactory { Target = hashCodeMethodLogMessageFactory };

            return elapsedMethodLogMessageFactory;
        }
    }
}
