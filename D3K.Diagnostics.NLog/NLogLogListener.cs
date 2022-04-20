using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.NLogExtensions
{
    public class NLogLogListener : ILogListener
    {
        readonly NLog.ILogger _logger;
        readonly ILoggerParamsFactory _nlogParamsFactory;

        public NLogLogListener(NLog.ILogger logger, ILoggerParamsFactory nlogParamsFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _nlogParamsFactory = nlogParamsFactory ?? throw new ArgumentNullException();
        }

        public void Log(object sender, LogEventArgs e)
        {
            var (nlogMessageTemplate, nlogPropertyValues) = _nlogParamsFactory.CreateLoggerParams(e.Message);

            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    _logger.Debug(nlogMessageTemplate, nlogPropertyValues);
                    break;

                case LogSeverity.Info:
                    _logger.Info(nlogMessageTemplate, nlogPropertyValues);
                    break;

                case LogSeverity.Warning:
                    _logger.Warn(nlogMessageTemplate, nlogPropertyValues);
                    break;

                case LogSeverity.Error:
                    _logger.Error(e.Message["Exception"] as Exception, nlogMessageTemplate, nlogPropertyValues);
                    break;

                case LogSeverity.Fatal:
                    _logger.Fatal(nlogMessageTemplate, nlogPropertyValues);
                    break;
            }
        }
    }
}
