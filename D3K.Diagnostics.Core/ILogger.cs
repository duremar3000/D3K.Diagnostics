using System;
using System.Collections.Generic;

namespace D3K.Diagnostics.Core
{
    public interface ILogger
    {
        void Debug(object msg);

        void Info(object msg);

        void Warning(object msg);

        void Error(object msg, Exception exp);

        void Error(object msg);
    }
}
