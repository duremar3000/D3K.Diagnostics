using System;
using System.Collections.Generic;
using System.Linq;

using Lamar;
using Lamar.DynamicInterception;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityBehavior<TLogContext>(this ServiceRegistry serviceRegistry, string name, string methodIdentityKey) where TLogContext : class, ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodIdentityBehavior>().Named(name)
                .Ctor<string>().Is(methodIdentityKey)
                .Ctor<IMethodIdentityProvider>().IsNamedInstance(name);            
            serviceRegistry.For<IMethodIdentityProvider>().Use<MethodIdentityProvider>().Named(name)
                .Ctor<ILogContext>().IsNamedInstance(name);
            serviceRegistry.For<ILogContext>().Use<TLogContext>().Named(name);
        }

        public static void RegisterMethodIdentityAsyncBehavior<TLogContext>(this ServiceRegistry serviceRegistry, string name, string methodIdentityKey) where TLogContext : class, ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            serviceRegistry.For<IInterceptionBehavior>().Use<MethodIdentityAsyncBehavior>().Named(name)
                .Ctor<string>().Is(methodIdentityKey)
                .Ctor<IMethodIdentityProvider>().IsNamedInstance(name);
            serviceRegistry.For<IMethodIdentityProvider>().Use<MethodIdentityProvider>().Named(name)
                .Ctor<ILogContext>().IsNamedInstance(name);
            serviceRegistry.For<ILogContext>().Use<TLogContext>().Named(name);
        }
    }
}
