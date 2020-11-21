using System;
using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Windsor
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityInterceptor<TLogContext>(this IWindsorContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodIdentityInterceptor>().Named(name).DependsOn(Dependency.OnValue("mehodIdentityKey", methodIdentityKey), Dependency.OnComponent("methodIdentityProvider", $"{name}MethodIdentityProvider")),
                Component.For<IMethodIdentityProvider>().ImplementedBy<MethodIdentityProvider>().Named($"{name}MethodIdentityProvider").DependsOn(Dependency.OnComponent("logContext", $"{name}LogContext")),
                Component.For<ILogContext>().ImplementedBy<TLogContext>().Named($"{name}LogContext"));
        }

        public static void RegisterMethodIdentityAsyncInterceptor<TLogContext>(this IWindsorContainer container, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodIdentityAsyncInterceptor>().Named(name).DependsOn(Dependency.OnValue("mehodIdentityKey", methodIdentityKey), Dependency.OnComponent("methodIdentityProvider", $"{name}MethodIdentityProvider")),
                Component.For<IMethodIdentityProvider>().ImplementedBy<MethodIdentityProvider>().Named($"{name}MethodIdentityProvider").DependsOn(Dependency.OnComponent("logContext", $"{name}LogContext")),
                Component.For<ILogContext>().ImplementedBy<TLogContext>().Named($"{name}LogContext"));
        }
    }
}
