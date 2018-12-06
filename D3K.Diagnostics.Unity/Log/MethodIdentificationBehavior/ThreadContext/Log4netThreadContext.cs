using System;
using System.Collections.Generic;
using System.Linq;

namespace D3K.Diagnostics.Unity.Log
{
    public class Log4netThreadContext : IThreadContext
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
