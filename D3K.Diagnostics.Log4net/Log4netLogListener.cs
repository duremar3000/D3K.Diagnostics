using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLogListener : ILogListener
    {
        readonly log4net.ILog _logger;

        public Log4netLogListener(log4net.ILog logger)
        {
            _logger = logger ?? throw new ArgumentNullException();
        }

        public void Log(object sender, LogEventArgs e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    var debugMsg = CreatetMessage(e.Message);
                    _logger.Debug(debugMsg);
                    break;

                case LogSeverity.Info:
                    var infoMsg = CreatetMessage(e.Message);
                    _logger.Info(infoMsg);
                    break;

                case LogSeverity.Warning:
                    var warningMsg = CreatetMessage(e.Message);
                    _logger.Warn(warningMsg);
                    break;

                case LogSeverity.Error:
                    var errorMsg = CreatetMessage(e.Message);
                    _logger.Error(errorMsg);
                    break;

                case LogSeverity.Fatal:
                    var fatalMsg = CreatetMessage(e.Message);
                    _logger.Fatal(fatalMsg);
                    break;
            }
        }

        private static string CreatetMessage(ILogMessage message)
        {
            var sb = new StringBuilder(message.ToString());

            return sb.ToString();
        }
    }
}
