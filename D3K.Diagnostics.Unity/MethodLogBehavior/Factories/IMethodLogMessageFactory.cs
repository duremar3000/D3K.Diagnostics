using System;
using System.Collections.Generic;
using System.Text;

using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    public interface IMethodLogMessageFactory
    {
        (string message, object correlationState) CreateInputMethodLogMessage(IMethodInvocation input);

        string CreateOutputMethodLogMessage(IMethodReturn methodReturn, object correlationState);
    }
}
