using System;
using System.Collections.Generic;
using System.Linq;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLoggingThreadContext : ILoggingContext
    {
        public object this[string key]
        {
            get
            {
                return log4net.ThreadContext.Properties[key];
            }
            set
            {
                log4net.ThreadContext.Properties[key] = value;
            }
        }
    }
}
