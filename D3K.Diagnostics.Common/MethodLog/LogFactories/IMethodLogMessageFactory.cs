using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface IMethodLogMessageFactory
    {
        (ILogMessage logMessage, object correlationState) CreateInputMethodLogMessage(MethodInput input);

        ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, object correlationState);
    }
}
