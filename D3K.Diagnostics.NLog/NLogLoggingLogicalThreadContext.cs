using System;
using System.Collections.Generic;
using System.Linq;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.NLogExtensions
{
    public class NLogLoggingLogicalThreadContext : ILoggingContext
    {
        public object this[string key]
        {
            get
            {
                return NLog.MappedDiagnosticsLogicalContext.GetObject(key);
            }
            set
            {
                NLog.MappedDiagnosticsLogicalContext.Set(key, value);
            }
        }
    }
}
