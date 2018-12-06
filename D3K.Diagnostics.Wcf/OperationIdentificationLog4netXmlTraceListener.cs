using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using D3K.Diagnostics.Log4net;

namespace D3K.Diagnostics.Wcf
{
    public class OperationIdentificationLog4netXmlTraceListener : Log4netXmlTraceListener
    {
        readonly string _operationIdentityKey;

        public OperationIdentificationLog4netXmlTraceListener(string loggerName) 
            : base(loggerName)
        {
            _operationIdentityKey = ConfigurationManager.AppSettings["OperationIdentityKey"];
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            var oid = log4net.ThreadContext.Properties[_operationIdentityKey]?.ToString();
            if (oid == null)
            {
                oid = CreateOperationIdentity();

                log4net.ThreadContext.Properties[_operationIdentityKey] = oid;
            }

            var isEndOperation = oid.EndsWith("[end]") == true;
            if (isEndOperation)
            {
                oid = oid.Replace("[end]", "");

                log4net.ThreadContext.Properties[_operationIdentityKey] = oid;
            }

            try
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
            finally
            {
                if (isEndOperation)
                {
                    log4net.ThreadContext.Properties[_operationIdentityKey] = null;
                }
            }
        }

        private static string CreateOperationIdentity()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x").PadLeft(8, 'x');
        }
    }
}
