using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Unity
{
    public interface ILogValueMapper
    {
        object Map(object sourceTraceValue);
    }
}
