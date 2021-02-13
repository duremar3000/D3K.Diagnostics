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

            container.Register<MethodLogInterceptor>();
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => logger.Attach(container.GetInstance<ILogListener>()));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>();
            container.Register(() => CreateLogMessageSettings());
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
        }

        public static void RegisterMethodLogInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogInterceptor>();
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => logger.Attach(container.GetInstance<ILogListener>()));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>();
            container.Register(() => CreateLogMessageSettings());
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this Container container, string loggerName) where TLogListenerFactory : class, ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>();
            container.Register<ILogger, Logger>();
            container.Register<ILogListenerFactory, TLogListenerFactory>();
            container.RegisterSingleton(() => container.GetInstance<ILogListenerFactory>().CreateLogListener(loggerName));
            container.RegisterInitializer<Logger>(logger => logger.Attach(container.GetInstance<ILogListener>()));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>();
            container.Register(() => CreateLogMessageSettings());
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
        }

        public static void RegisterMethodLogAsyncInterceptor(this Container container, ILogListener logListener)
        {
            if (logListener == null)
                throw new ArgumentException();

            container.Register<MethodLogAsyncInterceptor>();
            container.Register<ILogger, Logger>();
            container.RegisterSingleton(() => logListener);
            container.RegisterInitializer<Logger>(logger => logger.Attach(container.GetInstance<ILogListener>()));
            container.Register<IMethodLogMessageFactory, MethodLogMessageFactory>();
            container.Register(() => CreateLogMessageSettings());
            container.Register<ILogValueMapper, LogValueMapper>();
            container.Register<ILogValueMapperConfigurator, DefaultLogValueMapperConfigurator>();
        }

        private static ILogMessageSettings CreateLogMessageSettings()
        {
            return new LogMessageSettings
            {
                InputLogMessageTemplate = ">>{{ClassName}}.{{MethodName}}>> InputArgs: {{InputArgs}}",
                OutputLogMessageTemplate = "<<{{ClassName}}.{{MethodName}}<< ReturnValue: {{ReturnValue}}",
                ErrorLogMessageTemplate = "<<{{ClassName}}.{{MethodName}}<< Exception: {{Exception}}"
            };
        }
    }
}
