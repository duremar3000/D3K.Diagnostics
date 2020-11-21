using System;
using System.Collections.Generic;
using System.Linq;

using LightInject;
using LightInject.Interception;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.LightInject
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityInterceptor<TLogContext>(this IServiceContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register<IInterceptor>(sf => new MethodIdentityInterceptor(methodIdentityKey, sf.GetInstance<IMethodIdentityProvider>(name)), name);
            container.Register<IMethodIdentityProvider, MethodIdentityProvider>(name);
            container.Register<ILogContext, TLogContext>(name);
        }

        public static void RegisterMethodIdentityAsyncInterceptor<TLogContext>(this IServiceContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register<IInterceptor>(sf => new MethodIdentityAsyncInterceptor(methodIdentityKey, sf.GetInstance<IMethodIdentityProvider>(name)), name);
            container.Register<IMethodIdentityProvider, MethodIdentityProvider>(name);
            container.Register<ILogContext, TLogContext>(name);
        }
    }
}
