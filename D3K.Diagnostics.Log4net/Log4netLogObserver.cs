using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLogObserver : ILogObserver
    {
        readonly log4net.ILog _log;

        public Log4netLogObserver(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            _log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly(), loggerName);
        }

        public void Log(object sender, LogEventArgs e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    var debugMsg = CreatetMessage(e.Message);
                    _log.Debug(debugMsg);
                    break;

                case LogSeverity.Info:
                    var infoMsg = CreatetMessage(e.Message);
                    _log.Info(infoMsg);
                    break;

                case LogSeverity.Warning:
                    var warningMsg = CreatetMessage(e.Message);
                    _log.Warn(warningMsg);
                    break;

                case LogSeverity.Error:
                    var errorMsg = CreatetMessage(e.Message, e.Exception);
                    _log.Error(errorMsg);
                    break;

                case LogSeverity.Fatal:
                    var fatalMsg = CreatetMessage(e.Message);
                    _log.Fatal(fatalMsg);
                    break;
            }
        }

        private static string CreatetMessage(string message, Exception exception = null)
        {
            var sb = new StringBuilder(message);

            if (exception != null)
            {
                sb.AppendLine();
                sb.AppendLine(exception.ToString());
            }

            return sb.ToString();
        }
    }
}
