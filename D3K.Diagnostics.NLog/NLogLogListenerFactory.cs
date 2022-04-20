using System;
using System.Collections.Generic;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.NLogExtensions
{
    public class NLogLogListenerFactory : ILogListenerFactory
    {
        static readonly ILoggerParamsFactory _nlogParamsFactory;

        static NLogLogListenerFactory()
        {
            _nlogParamsFactory = new CorrLoggerParamsFactory
            {
                Target = new CorrExceptionNLogParamsFactory
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

            var logger = NLog.LogManager.GetLogger(loggerName);

            return new NLogLogListener(logger, _nlogParamsFactory);
        }
    }
}
