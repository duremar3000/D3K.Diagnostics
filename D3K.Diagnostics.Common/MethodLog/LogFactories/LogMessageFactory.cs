using System;
using System.Collections.Generic;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Common
{
    public class LogMessageFactory : ILogMessageFactory
    {
        public ILogMessage CreateLogMessage(string messageTemplate)
        {
            return new LogMessage(messageTemplate);
        }
    }
}
