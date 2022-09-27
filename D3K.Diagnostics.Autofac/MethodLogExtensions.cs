using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

using Autofac;
using Autofac.Core;

using D3K.Diagnostics.Castle;
using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Autofac
{
    public static class MethodLogExtensions
    {
        public static void RegisterMethodLogInterceptor<TLogListenerFactory>(this ContainerBuilder builder, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            builder.RegisterType<MethodLogInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterType<TLogListenerFactory>().Named<ILogListenerFactory>(name);
            builder.Register(c => c.ResolveNamed<ILogListenerFactory>(name).CreateLogListener(loggerName)).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<LogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor<TLogListenerFactory>(this ContainerBuilder builder, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            builder.RegisterType<MethodLogInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterType<TLogListenerFactory>().Named<ILogListenerFactory>(name);
            builder.Register(c => c.ResolveNamed<ILogListenerFactory>(name).CreateLogListener(loggerName)).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            builder.RegisterType<HashCodeMethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory").OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<HashCodeLogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogInterceptor(this ContainerBuilder builder, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            builder.RegisterType<MethodLogInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterInstance(logListener).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<LogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogInterceptor(this ContainerBuilder builder, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            builder.RegisterType<MethodLogInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterInstance(logListener).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            builder.RegisterType<HashCodeMethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory").OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<HashCodeLogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogAsyncInterceptor<TLogListenerFactory>(this ContainerBuilder builder, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            builder.RegisterType<MethodLogAsyncInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterType<TLogListenerFactory>().Named<ILogListenerFactory>(name);
            builder.Register(c => c.ResolveNamed<ILogListenerFactory>(name).CreateLogListener(loggerName)).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<LogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor<TLogListenerFactory>(this ContainerBuilder builder, string name, string loggerName) where TLogListenerFactory : ILogListenerFactory, new()
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            builder.RegisterType<MethodLogAsyncInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterType<TLogListenerFactory>().Named<ILogListenerFactory>(name);
            builder.Register(c => c.ResolveNamed<ILogListenerFactory>(name).CreateLogListener(loggerName)).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            builder.RegisterType<HashCodeMethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory").OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<HashCodeLogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterMethodLogAsyncInterceptor(this ContainerBuilder builder, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            builder.RegisterType<MethodLogAsyncInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterInstance(logListener).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<LogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }

        public static void RegisterHashCodeMethodLogAsyncInterceptor(this ContainerBuilder builder, string name, ILogListener logListener)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException();

            if (logListener == null)
                throw new ArgumentException();

            builder.RegisterType<MethodLogAsyncInterceptor>().Named<IInterceptor>(name).SingleInstance()
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logger" && pi.ParameterType == typeof(ILogger), (pi, ctx) => ctx.ResolveNamed<ILogger>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "methodLogMessageFactory" && pi.ParameterType == typeof(IMethodLogMessageFactory), (pi, ctx) => ctx.ResolveNamed<IMethodLogMessageFactory>(name)));
            builder.RegisterType<Logger>().Named<ILogger>(name).OnActivating(e => e.Instance.Attach(e.Context.ResolveNamed<ILogListener>(name)));
            builder.RegisterInstance(logListener).Named<ILogListener>(name).SingleInstance();
            builder.RegisterType<ElapsedMethodLogMessageFactory>().Named<IMethodLogMessageFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory"));
            builder.RegisterType<HashCodeMethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}HashCodeMethodLogMessageFactory").OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory"));
            builder.RegisterType<MethodLogMessageFactory>().Named<IMethodLogMessageFactory>($"{name}MethodLogMessageFactory")
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageSettings" && pi.ParameterType == typeof(ILogMessageSettings), (pi, ctx) => ctx.ResolveNamed<ILogMessageSettings>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "argListObjectMapper" && pi.ParameterType == typeof(IArgListObjectMapper), (pi, ctx) => ctx.ResolveNamed<IArgListObjectMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "typeShortNameFactory" && pi.ParameterType == typeof(ITypeShortNameFactory), (pi, ctx) => ctx.ResolveNamed<ITypeShortNameFactory>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logMessageFactory" && pi.ParameterType == typeof(ILogMessageFactory), (pi, ctx) => ctx.ResolveNamed<ILogMessageFactory>(name)));
            builder.RegisterType<HashCodeLogMessageSettings>().Named<ILogMessageSettings>(name);
            builder.RegisterType<ArgListObjectMapper>().Named<IArgListObjectMapper>(name)
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapper" && pi.ParameterType == typeof(ILogValueMapper), (pi, ctx) => ctx.ResolveNamed<ILogValueMapper>(name)))
                .WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectFactory" && pi.ParameterType == typeof(IDynamicArgListObjectFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectFactory>(name)));
            builder.RegisterType<LogValueMapper>().Named<ILogValueMapper>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "logValueMapperConfigurator" && pi.ParameterType == typeof(ILogValueMapperConfigurator), (pi, ctx) => ctx.ResolveNamed<ILogValueMapperConfigurator>(name)));
            builder.RegisterType<DynamicArgListObjectFactory>().Named<IDynamicArgListObjectFactory>(name).WithParameter(new ResolvedParameter((pi, ctx) => pi.Name == "dynamicArgListObjectTypeFactory" && pi.ParameterType == typeof(IDynamicArgListObjectTypeFactory), (pi, ctx) => ctx.ResolveNamed<IDynamicArgListObjectTypeFactory>(name)));
            builder.RegisterType<CachingDynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory"));
            builder.RegisterType<DynamicArgListObjectTypeFactory>().Named<IDynamicArgListObjectTypeFactory>($"{name}DynamicArgListObjectTypeFactory");
            builder.RegisterType<CachingTypeShortNameFactory>().Named<ITypeShortNameFactory>(name).OnActivating(e => e.Instance.Target = e.Context.ResolveNamed<ITypeShortNameFactory>($"{name}TypeShortNameFactory"));
            builder.RegisterType<TypeShortNameFactory>().Named<ITypeShortNameFactory>($"{name}TypeShortNameFactory");
            builder.RegisterType<LogMessageFactory>().Named<ILogMessageFactory>(name);
            builder.RegisterType<DefaultLogValueMapperConfigurator>().Named<ILogValueMapperConfigurator>(name);
        }
    }
}