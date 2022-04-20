using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core
{
    public interface ILogger
    {
        void Debug(ILogMessage msg);

        void Info(ILogMessage msg);

        void Warning(ILogMessage msg);

        void Error(ILogMessage msg);
    }
}
