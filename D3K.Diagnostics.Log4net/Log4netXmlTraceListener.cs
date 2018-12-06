using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Log4net
{
    public class Log4netXmlTraceListener : XmlWriterTraceListener
    {
        readonly StringBuilder _sb;
        readonly log4net.ILog _log;

        public Log4netXmlTraceListener(string loggerName) 
            : this(new StringBuilder())
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            _log = log4net.LogManager.GetLogger(loggerName);
        }

        private Log4netXmlTraceListener(StringBuilder sb) 
            : base(new StringWriter(sb))
        {
            _sb = sb;
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            lock (_sb)
            {
                _sb.Clear();

                base.TraceData(eventCache, source, eventType, id, data);

                var msg = _sb.ToString();
                _log.Debug(msg);
            }
        }
    }
}
