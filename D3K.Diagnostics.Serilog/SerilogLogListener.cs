using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class SerilogLogListener : ILogListener
    {
        readonly Serilog.ILogger _logger;
        readonly ILoggerParamsFactory _serilogParamsFactory;

        public SerilogLogListener(Serilog.ILogger logger, ILoggerParamsFactory serilogParamsFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _serilogParamsFactory = serilogParamsFactory ?? throw new ArgumentNullException();
        }

        public void Log(object sender, LogEventArgs e)
        {
            var (serilogMessageTemplate, serilogPropertyValues) = _serilogParamsFactory.CreateLoggerParams(e.Message);

            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    _logger.Debug(serilogMessageTemplate, serilogPropertyValues);
                    break;

                case LogSeverity.Info:
                    _logger.Information(serilogMessageTemplate, serilogPropertyValues);
                    break;

                case LogSeverity.Warning:
                    _logger.Warning(serilogMessageTemplate, serilogPropertyValues);
                    break;

                case LogSeverity.Error:
                    _logger.Error(e.Message["Exception"] as Exception, serilogMessageTemplate, serilogPropertyValues);
                    break;

                case LogSeverity.Fatal:
                    _logger.Fatal(serilogMessageTemplate, serilogPropertyValues);
                    break;
            }
        }
    }
}
