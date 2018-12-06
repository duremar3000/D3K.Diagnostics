using System;
using System.Collections.Generic;
using System.Linq;

namespace D3K.Diagnostics.Unity.Log
{
    public interface IThreadContext
    {
        object this[string key] { get; set; }
    }
}
