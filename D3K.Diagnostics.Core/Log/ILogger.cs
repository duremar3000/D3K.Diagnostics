using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core.Log
{
    public interface ILogger
    {
        void Debug(string msg);

        void Info(string msg);

        void Warning(string msg);

        void Error(string msg, Exception exp);

        void Error(string msg);
    }
}
