using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

using Serilog;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class XmlSerilogLogListenerFactory : ILogListenerFactory
    {
        static readonly ILoggerParamsFactory _serilogParamsFactory;
        static readonly ConcurrentDictionary<string, Serilog.Core.Logger> _loggers = new ConcurrentDictionary<string, Serilog.Core.Logger>();

        static XmlSerilogLogListenerFactory()
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
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.With(new RemoveTypeTagEnricher())
                .ReadFrom.AppSettings(loggerName, "serilog.config")
                ;

            var logger = loggerConfiguration.CreateLogger();

            return logger;
        }
    }
}
