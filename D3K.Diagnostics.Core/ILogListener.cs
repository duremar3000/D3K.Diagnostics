using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core
{
    public interface ILogListener
    {
        void Log(object sender, LogEventArgs e);
    }
}
