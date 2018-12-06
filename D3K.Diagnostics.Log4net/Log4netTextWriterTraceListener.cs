using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Log4net
{
    public class Log4netTextWriterTraceListener : TextWriterTraceListener
    {
        readonly Context _context = new Context();
        readonly log4net.ILog _log;

        public Log4netTextWriterTraceListener(string loggerName)
            : base()
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            _log = log4net.LogManager.GetLogger(loggerName);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            lock (_context)
            {
                _context.EventType = eventType;

                base.TraceEvent(eventCache, source, eventType, id, message);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            lock (_context)
            {
                _context.EventType = eventType;

                base.TraceEvent(eventCache, source, eventType, id, format, args);
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            lock (_context)
            {
                base.TraceEvent(eventCache, source, eventType, id);
            }
        }

        public override void WriteLine(string message)
        {
            lock (_context)
            {
                switch (_context.EventType)
                {
                    case TraceEventType.Critical:
                        _log.Fatal(message);
                        break;
                    case TraceEventType.Error:
                        _log.Error(message);
                        break;
                    case TraceEventType.Warning:
                        _log.Warn(message);
                        break;
                    default:
                        _log.Info(message);
                        break;
                }
            }
        }

        class Context
        {
            public TraceEventType EventType { get; set; }
        }
    }
}
