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
        public LogEventArgs(LogSeverity severity, ILogMessage message, DateTime date)
        {
            Severity = severity;
            Message = message;
            Date = date;
        }

        public LogSeverity Severity { get; private set; }

        public ILogMessage Message { get; private set; }

        public DateTime Date { get; private set; }

        public string SeverityString
        {
            get { return Severity.ToString("G"); }
        }
    }
}
