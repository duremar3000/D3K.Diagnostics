using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Core.Log
{
    public class ObserverTraceLog : ILog
    {
        readonly TraceSource _traceSource;

        public ObserverTraceLog(string traceSourceName)
        {
            if (string.IsNullOrEmpty(traceSourceName))
                throw new ArgumentException();

            _traceSource = new TraceSource(traceSourceName);
        }

        public void Log(object sender, LogEventArgs e)
        {
            switch (e.Severity)
            {
                case LogSeverity.Debug:
                    var debugMsg = CreatetMessage(e.Message, e.Exception);
                    _traceSource.TraceEvent(TraceEventType.Verbose, 0, debugMsg);
                    break;

                case LogSeverity.Info:
                    var infoMsg = CreatetMessage(e.Message, e.Exception);
                    _traceSource.TraceInformation(infoMsg);
                    break;

                case LogSeverity.Warning:
                    var warningMsg = CreatetMessage(e.Message, e.Exception);
                    _traceSource.TraceEvent(TraceEventType.Warning, 0, warningMsg);
                    break;

                case LogSeverity.Error:
                    var errorMsg = CreatetMessage(e.Message, e.Exception);
                    _traceSource.TraceEvent(TraceEventType.Error, 0, errorMsg);
                    break;
                    
                case LogSeverity.Fatal:
                    var fatalMsg = CreatetMessage(e.Message, e.Exception);
                    _traceSource.TraceEvent(TraceEventType.Critical, 0, fatalMsg);
                    break;
            }
        }

        private static string CreatetMessage(string message, Exception exception)
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
