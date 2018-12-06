using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core.Log
{
    public interface ILog
    {
        void Log(object sender, LogEventArgs e);
    }
}
