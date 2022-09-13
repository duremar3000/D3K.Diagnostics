using System;
using System.Collections.Generic;
using System.Linq;

using Ninject;
using Ninject.Extensions.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Ninject
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogInterceptor<TLogListenerFactory>(this IKernel kernel, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListenerFactory>().To<TLogListenerFactory>().Named(name);
            kernel.Bind<ILogListener>().ToMethod(ctx => ctx.Kernel.Get<ILogListenerFactory>(name).CreateLogListener(loggerName)).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<LogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this IKernel kernel, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListenerFactory>().To<TLogListenerFactory>().Named(name);
            kernel.Bind<ILogListener>().ToMethod(ctx => ctx.Kernel.Get<ILogListenerFactory>(name).CreateLogListener(loggerName)).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<HashCodeLogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterMethodLogInterceptor(this IKernel kernel, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListener>().ToConstant(logListener).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<LogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor(this IKernel kernel, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListener>().ToConstant(logListener).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<HashCodeLogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this IKernel kernel, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogAsyncInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListenerFactory>().To<TLogListenerFactory>().Named(name);
            kernel.Bind<ILogListener>().ToMethod(ctx => ctx.Kernel.Get<ILogListenerFactory>(name).CreateLogListener(loggerName)).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<LogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this IKernel kernel, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogAsyncInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListenerFactory>().To<TLogListenerFactory>().Named(name);
            kernel.Bind<ILogListener>().ToMethod(ctx => ctx.Kernel.Get<ILogListenerFactory>(name).CreateLogListener(loggerName)).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<HashCodeLogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterMethodLogAsyncInterceptor(this IKernel kernel, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogAsyncInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListener>().ToConstant(logListener).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<LogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this IKernel kernel, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            kernel.Bind<IInterceptor>().To<MethodLogAsyncInterceptor>().InSingletonScope().Named(name);

            kernel.Bind<ILogger>().To<Logger>().WhenParentNamed(name).Named(name).OnActivation<Logger>((ctx, imp) => imp.Attach(ctx.Kernel.Get<ILogListener>(name)));
            kernel.Bind<ILogListener>().ToConstant(logListener).InSingletonScope().Named(name);
            kernel.Bind<IMethodLogMessageFactory>().To<ElapsedMethodLogMessageFactory>().WhenParentNamed(name).Named(name).WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").WithPropertyValue("Target", ctx => ctx.Kernel.Get<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            kernel.Bind<IMethodLogMessageFactory>().To<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory");
            kernel.Bind<ILogMessageSettings>().To<HashCodeLogMessageSettings>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapper>().To<LogValueMapper>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogMessageFactory>().To<LogMessageFactory>().WhenParentNamed($"{name}MethodLogMessageFactory").Named(name);
            kernel.Bind<ILogValueMapperConfigurator>().To<DefaultLogValueMapperConfigurator>().WhenParentNamed(name).Named(name);
        }
    }
}
