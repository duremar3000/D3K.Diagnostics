﻿using System;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class SerilogLogListener: ILogListener
    {
        readonly Serilog.ILogger _logger;

        public SerilogLogListener(Serilog.ILogger logger)
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
                    _logger.Information(infoMsg);
                    break;

                case LogSeverity.Warning:
                    var warningMsg = CreatetMessage(e.Message);
                    _logger.Warning(warningMsg);
                    break;

                case LogSeverity.Error:
                    var errorMsg = CreatetMessage(e.Message, e.Exception);
                    _logger.Error(errorMsg);
                    break;

                case LogSeverity.Fatal:
                    var fatalMsg = CreatetMessage(e.Message);
                    _logger.Fatal(fatalMsg);
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
