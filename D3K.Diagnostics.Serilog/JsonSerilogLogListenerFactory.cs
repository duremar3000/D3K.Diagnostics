using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

using Serilog;

using Microsoft.Extensions.Configuration;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class JsonSerilogLogListenerFactory : ILogListenerFactory
    {
        static readonly ILoggerParamsFactory _serilogParamsFactory;
        static readonly ConcurrentDictionary<string, Serilog.Core.Logger> _loggers = new ConcurrentDictionary<string, Serilog.Core.Logger>();

        static JsonSerilogLogListenerFactory()
        {
            _serilogParamsFactory = new CorrLoggerParamsFactory
            {
                Target = new CorrExceptionSerilogParamsFactory
                {
                    Target = new LoggerParamsFactory(
                        new CachingNLogMessageTemplateMapper
                        {
                            Target = new LoggerMessageTemplateMapper()
                        },
                        new LoggerPropertyValuesFactory(
                            new CachingLoggerPropertyNamesFactory
                            {
                                Target = new LoggerPropertyNamesFactory()
                            }))
                }
            };
        }

        public ILogListener CreateLogListener(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            var logger = _loggers.GetOrAdd(loggerName, CreateSerilogLogger(loggerName));

            return new SerilogLogListener(logger, _serilogParamsFactory);
        }

        private Serilog.Core.Logger CreateSerilogLogger(string loggerName)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.json")
                .Build();

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.With(new RemoveTypeTagEnricher())
                .ReadFrom.Configuration(configuration)
                ;

            var logger = loggerConfiguration.CreateLogger();

            return logger;
        }
    }
}
