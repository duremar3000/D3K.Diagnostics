using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Common
{
    public interface ILogValueMapper
    {
        object Map(object sourceTraceValue);
    }
}
