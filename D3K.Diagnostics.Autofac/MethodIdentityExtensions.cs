using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

using Autofac;
using Autofac.Core;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Autofac
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityInterceptor<TLogContext>(this ContainerBuilder builder, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            builder.RegisterType<MethodIdentityInterceptor>().Named<IInterceptor>(name)
                .WithParameter("mehodIdentityKey", methodIdentityKey)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodIdentityProvider" && pi.ParameterType == typeof(IMethodIdentityProvider), (pi, ctx) => ctx.ResolveNamed<IMethodIdentityProvider>(name)));

            builder.RegisterType<MethodIdentityProvider>().Named<IMethodIdentityProvider>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logContext" && pi.ParameterType == typeof(ILogContext), (pi, ctx) => ctx.ResolveNamed<ILogContext>(name)));
            builder.RegisterType<TLogContext>().Named<ILogContext>(name);
        }

        public static void RegisterMethodIdentityAsyncInterceptor<TLogContext>(this ContainerBuilder builder, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            builder.RegisterType<MethodIdentityAsyncInterceptor>().Named<IInterceptor>(name)
                .WithParameter("mehodIdentityKey", methodIdentityKey)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodIdentityProvider" && pi.ParameterType == typeof(IMethodIdentityProvider), (pi, ctx) => ctx.ResolveNamed<IMethodIdentityProvider>(name)));

            builder.RegisterType<MethodIdentityProvider>().Named<IMethodIdentityProvider>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logContext" && pi.ParameterType == typeof(ILogContext), (pi, ctx) => ctx.ResolveNamed<ILogContext>(name)));
            builder.RegisterType<TLogContext>().Named<ILogContext>(name);
        }
    }
}
