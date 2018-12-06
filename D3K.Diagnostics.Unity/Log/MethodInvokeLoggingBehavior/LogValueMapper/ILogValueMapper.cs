using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Unity.Log
{
    public interface ILogValueMapper
    {
        object Map(object sourceTraceValue);
    }
}
