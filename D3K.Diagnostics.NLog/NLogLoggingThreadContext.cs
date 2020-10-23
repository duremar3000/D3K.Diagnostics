using System;
using System.Collections.Generic;
using System.Linq;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.NLogExtensions
{
    public class NLogLoggingThreadContext : ILoggingContext
    {
        public object this[string key]
        {
            get
            {
                return NLog.MappedDiagnosticsContext.GetObject(key);
            }
            set
            {
                NLog.MappedDiagnosticsContext.Set(key, value);
            }
        }
    }
}
