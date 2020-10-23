using System;
using System.Collections.Generic;
using System.Linq;

using Unity;
using Unity.Injection;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Unity
{
    public static class MethodIdentityExtensions
    {
        public static IUnityContainer RegisterMethodIdentityBehavior<TThreadLocalStorageLoggerProvider>(this IUnityContainer container, string name, string methodIdentityKey) where TThreadLocalStorageLoggerProvider : ILoggingContext
        {
            container
                .RegisterType<MethodIdentityBehavior>(name, new InjectionConstructor(methodIdentityKey, new ResolvedParameter<ILoggingContext>(name)))
                .RegisterType<ILoggingContext, TThreadLocalStorageLoggerProvider>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodIdentityAsyncBehavior<TThreadLocalStorageLoggerProvider>(this IUnityContainer container, string name, string methodIdentityKey) where TThreadLocalStorageLoggerProvider : ILoggingContext
        {
            container
                .RegisterType<MethodIdentityAsyncBehavior>(name, new InjectionConstructor(methodIdentityKey, new ResolvedParameter<ILoggingContext>(name)))
                .RegisterType<ILoggingContext, TThreadLocalStorageLoggerProvider>(name);

            return container;
        }
    }
}
