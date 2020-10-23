using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Core
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(LogSeverity severity, string message, Exception exception, DateTime date)
        {
            Severity = severity;
            Message = message;
            Exception = exception;
            Date = date;
        }

        public LogSeverity Severity { get; private set; }

        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public DateTime Date { get; private set; }

        public string SeverityString
        {
            get { return Severity.ToString("G"); }
        }
    }
}
