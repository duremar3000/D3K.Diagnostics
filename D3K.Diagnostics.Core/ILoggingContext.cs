using System;
using System.Collections.Generic;
using System.Linq;

namespace D3K.Diagnostics.Core
{
    public interface ILoggingContext
    {
        object this[string key] { get; set; }
    }
}
