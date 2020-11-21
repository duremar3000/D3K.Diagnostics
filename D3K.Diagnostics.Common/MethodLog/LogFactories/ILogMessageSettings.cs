using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface ILogMessageSettings
    {
        string InputLogMessageTemplate { get; }

        string OutputLogMessageTemplate { get; }

        string ErrorLogMessageTemplate { get; }
    }
}
