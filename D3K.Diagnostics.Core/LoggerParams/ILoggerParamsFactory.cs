using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public interface ILoggerParamsFactory
    {
        (string logMessageTemplate, object[] logPropertyValues) CreateLoggerParams(ILogMessage logMessage);
    }
}
