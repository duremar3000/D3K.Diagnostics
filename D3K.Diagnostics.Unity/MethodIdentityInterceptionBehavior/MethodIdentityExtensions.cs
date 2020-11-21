using System;
using System.Collections.Generic;
using System.Linq;

using Unity;
using Unity.Injection;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    public static class MethodIdentityExtensions
    {
        public static IUnityContainer RegisterMethodIdentityInterceptionBehavior<TLogContext>(this IUnityContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container
                .RegisterType<MethodIdentityInterceptionBehavior>(name, new InjectionConstructor(methodIdentityKey, new ResolvedParameter<IMethodIdentityProvider>(name)))
                .RegisterType<IMethodIdentityProvider, MethodIdentityProvider>(name, new InjectionConstructor(new ResolvedParameter<ILogContext>(name)))
                .RegisterType<ILogContext, TLogContext>(name);

            return container;
        }

        public static IUnityContainer RegisterMethodIdentityAsyncInterceptionBehavior<TLogContext>(this IUnityContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container
                .RegisterType<MethodIdentityAsyncInterceptionBehavior>(name, new InjectionConstructor(methodIdentityKey, new ResolvedParameter<IMethodIdentityProvider>(name)))
                .RegisterType<IMethodIdentityProvider, MethodIdentityProvider>(name, new InjectionConstructor(new ResolvedParameter<ILogContext>(name)))
                .RegisterType<ILogContext, TLogContext>(name);

            return container;
        }
    }
}
