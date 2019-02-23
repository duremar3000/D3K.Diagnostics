﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using Unity.Injection;

using D3K.Diagnostics.Core.Log;
using D3K.Diagnostics.Log4net.Demo.Wcf.Client.ServiceReference;

namespace D3K.Diagnostics.Log4net.Demo.Wcf.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer()
                .RegisterType<ILogger, Logger>(new InjectionMethod(nameof(Logger.Attach), typeof(ILog)))
                .RegisterType<ILog, ObserverTraceLog>(new InjectionConstructor("debug"))

                .RegisterType<IService, LoggingService>(new InjectionConstructor(new ResolvedParameter<IService>("service"), typeof(ILogger)))
                .RegisterType<IService, ServiceClient>("service", new InjectionConstructor());

            var service = container.Resolve<IService>();

            var res = service.DoWork(new DoWorkArgs { Arg1 = "Hello", Arg2 = "World" });

            Console.WriteLine(res.Value);

            Console.ReadLine();
        }
    }
}