using System;
using System.Collections.Generic;
using System.Linq;

using SimpleInjector;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.SimpleInjector
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityInterceptor<TLogContext>(this Container container, string methodIdentityKey) where TLogContext : class, ILogContext, new()
        {
            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register(() => new MethodIdentityInterceptor(methodIdentityKey, container.GetInstance<IMethodIdentityProvider>()));
            container.Register<IMethodIdentityProvider, MethodIdentityProvider>();
            container.Register<ILogContext, TLogContext>();
        }

        public static void RegisterMethodIdentityAsyncInterceptor<TLogContext>(this Container container, string methodIdentityKey) where TLogContext : class, ILogContext, new()
        {
            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register(() => new MethodIdentityAsyncInterceptor(methodIdentityKey, container.GetInstance<IMethodIdentityProvider>()));
            container.Register<IMethodIdentityProvider, MethodIdentityProvider>();
            container.Register<ILogContext, TLogContext>();
        }
    }
}
