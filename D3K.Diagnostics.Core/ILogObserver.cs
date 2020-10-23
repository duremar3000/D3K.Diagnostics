using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core
{
    public interface ILogObserver
    {
        void Log(object sender, LogEventArgs e);
    }
}
