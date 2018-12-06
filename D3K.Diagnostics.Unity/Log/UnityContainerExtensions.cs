using System;
using System.Collections.Generic;
using System.Linq;

using Unity;
using Unity.Injection;

using D3K.Diagnostics.Core.Log;

namespace D3K.Diagnostics.Unity.Log
{
    public static class UnityContainerExtensions
    {
        public static IUnityContainer RegisterMethodInvokeLoggingBehavior(this IUnityContainer container, string behaviorName, string traceSourceName)
        {
            if (string.IsNullOrEmpty(behaviorName))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(traceSourceName))
                throw new ArgumentException();

            var loggerName = $"logger[{Guid.NewGuid()}]";

            container
                .RegisterType<MethodInvokeLoggingBehavior>(behaviorName, new InjectionConstructor(new ResolvedParameter<ILogger>(loggerName), typeof(ILogValueMapper)))
                .RegisterType<ILogger, Logger>(loggerName, new InjectionMethod(nameof(Logger.Attach), typeof(ILog)))
                .RegisterType<ILog, ObserverTraceLog>(new InjectionConstructor(traceSourceName))
                .RegisterType<ILogValueMapper, LogValueMapper>();

            return container;
        }

        public static IUnityContainer RegisterMethodIdentificationBehavior(this IUnityContainer container, string methodIdentityKey)
        {
            container
                .RegisterType<MethodIdentificationBehavior>(new InjectionConstructor(methodIdentityKey, typeof(IThreadContext)))
                .RegisterType<IThreadContext, Log4netThreadContext>();

            return container;

        }
    }
}
