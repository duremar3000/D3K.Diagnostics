using System;
using System.Collections.Generic;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Common
{
    public interface ILogMessageFactory
    {
        ILogMessage CreateLogMessage(string messageTemplate);
    }
}
