using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public interface ILogContext
    {
        IDisposable PushProperty(string name, object value);

        object PeekProperty(string name);
    }
}
