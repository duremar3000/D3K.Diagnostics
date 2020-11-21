using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

using Ninject.Extensions.Interception;
using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Ninject
{
    public static class MethodIdentityExtensions
    {
        public static void RegisterMethodIdentityInterceptor<TLogContext>(this IKernel kernel, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodIdentityInterceptor>().Named(name).WithConstructorArgument("mehodIdentityKey", methodIdentityKey);
            kernel.Bind<IMethodIdentityProvider>().To<MethodIdentityProvider>().WhenParentNamed(name).Named(name);
            kernel.Bind<ILogContext>().To<TLogContext>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterMethodIdentityAsyncInterceptor<TLogContext>(this IKernel kernel, string name, string methodIdentityKey) where TLogContext : ILogContext, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(methodIdentityKey))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodIdentityAsyncInterceptor>().Named(name).WithConstructorArgument("mehodIdentityKey", methodIdentityKey);
            kernel.Bind<IMethodIdentityProvider>().To<MethodIdentityProvider>().WhenParentNamed(name).Named(name);
            kernel.Bind<ILogContext>().To<TLogContext>().WhenParentNamed(name).Named(name);
        }
    }
}
