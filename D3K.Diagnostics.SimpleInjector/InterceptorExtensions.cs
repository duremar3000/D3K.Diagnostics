using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Castle.DynamicProxy;

namespace SimpleInjector.InterceptorExtensions
{
    public static class InterceptorExtensions
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        private static readonly Func<Type, object, IInterceptor, object> createProxy =
            (p, t, i) => generator.CreateInterfaceProxyWithTarget(p, t, i);

        public static void InterceptWith<TInterceptor>(this Container container, Predicate<Type> predicate) where TInterceptor : class, IInterceptor
        {
            container.ExpressionBuilt += (s, e) => 
            {
                if (predicate(e.RegisteredServiceType))
                {
                    e.Expression = Expression.Convert(
                        Expression.Invoke(
                            Expression.Constant(createProxy),
                            Expression.Constant(e.RegisteredServiceType, typeof(Type)),
                            e.Expression,
                            container.GetRegistration(typeof(TInterceptor), true).BuildExpression()),
                        e.RegisteredServiceType);
                }
            };
        }
    }
}
