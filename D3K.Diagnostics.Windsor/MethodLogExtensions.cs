using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Windsor
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogInterceptor<TLogListenerFactory>(this IWindsorContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListenerFactory>().ImplementedBy<TLogListenerFactory>().Named($"{name}LogListenerFactory"),
                Component.For<ILogListener>().UsingFactoryMethod(kernel => kernel.Resolve<ILogListenerFactory>($"{name}LogListenerFactory").CreateLogListener(loggerName)).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<LogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this IWindsorContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListenerFactory>().ImplementedBy<TLogListenerFactory>().Named($"{name}LogListenerFactory"),
                Component.For<ILogListener>().UsingFactoryMethod(kernel => kernel.Resolve<ILogListenerFactory>($"{name}LogListenerFactory").CreateLogListener(loggerName)).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}HashCodeMethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<HashCodeLogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterMethodLogInterceptor(this IWindsorContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListener>().Instance(logListener).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<LogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterHashCodeMethodLogInterceptor(this IWindsorContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListener>().Instance(logListener).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}HashCodeMethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<HashCodeLogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this IWindsorContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogAsyncInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListenerFactory>().ImplementedBy<TLogListenerFactory>().Named($"{name}LogListenerFactory"),
                Component.For<ILogListener>().UsingFactoryMethod(kernel => kernel.Resolve<ILogListenerFactory>($"{name}LogListenerFactory").CreateLogListener(loggerName)).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<LogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this IWindsorContainer container, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogAsyncInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListenerFactory>().ImplementedBy<TLogListenerFactory>().Named($"{name}LogListenerFactory"),
                Component.For<ILogListener>().UsingFactoryMethod(kernel => kernel.Resolve<ILogListenerFactory>($"{name}LogListenerFactory").CreateLogListener(loggerName)).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}HashCodeMethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<HashCodeLogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterMethodLogAsyncInterceptor(this IWindsorContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogAsyncInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListener>().Instance(logListener).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<LogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this IWindsorContainer container, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            container.Register(
                Component.For<MethodLogAsyncInterceptor>().Named(name).DependsOn(ServiceOverride.ForKey<ILogger>().Eq($"{name}Logger")).DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}ElapsedMethodLogMessageFactory")),
                Component.For<ILogger>().UsingFactoryMethod(kernel => CreateLogger(kernel, $"{name}LogListener")).Named($"{name}Logger"),
                Component.For<ILogListener>().Instance(logListener).Named($"{name}LogListener").LifestyleSingleton(),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<ElapsedMethodLogMessageFactory>().Named($"{name}ElapsedMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}HashCodeMethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<HashCodeMethodLogMessageFactory>().Named($"{name}HashCodeMethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<IMethodLogMessageFactory>().Eq($"{name}MethodLogMessageFactory")),
                Component.For<IMethodLogMessageFactory>().ImplementedBy<MethodLogMessageFactory>().Named($"{name}MethodLogMessageFactory").DependsOn(ServiceOverride.ForKey<ILogValueMapper>().Eq($"{name}LogValueMapper"), ServiceOverride.ForKey<ILogMessageSettings>().Eq($"{name}LogMessageSettings")),
                Component.For<ILogMessageSettings>().ImplementedBy<HashCodeLogMessageSettings>().Named($"{name}LogMessageSettings"),
                Component.For<ILogValueMapper>().ImplementedBy<LogValueMapper>().Named($"{name}LogValueMapper").DependsOn(ServiceOverride.ForKey<ILogValueMapperConfigurator>().Eq($"{name}LogValueMapperConfigurator")),
                Component.For<ILogValueMapperConfigurator>().ImplementedBy<DefaultLogValueMapperConfigurator>().Named($"{name}LogValueMapperConfigurator"));
        }

        private static ILogger CreateLogger(IKernel kernel, string name)
        {
            var logger = new Logger();

            logger.Attach(kernel.Resolve<ILogListener>(name));

            return logger;
        }
    }
}
