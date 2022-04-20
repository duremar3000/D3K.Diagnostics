using System;
using System.Collections.Generic;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Common
{
    public class HashCodeMethodLogMessageFactory : IMethodLogMessageFactory
    {
        public IMethodLogMessageFactory Target { get; set; }

        public (ILogMessage logMessage, object correlationState) CreateInputMethodLogMessage(MethodInput methodInput)
        {
            var (logMessage, correlationState) = Target.CreateInputMethodLogMessage(methodInput);

            var targetHashCode = methodInput.Target.GetHashCode();

            logMessage.AddMessageProperty("HashCode", targetHashCode);

            return (logMessage, correlationState: new CorrelationState { TargetHashCode = targetHashCode, InnerCorrelationState = correlationState });
        }

        public ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, CorrelationState correlationState)
        {
            var logMessage = Target.CreateOutputMethodLogMessage(methodReturn, correlationState.InnerCorrelationState);

            logMessage.AddMessageProperty("HashCode", correlationState.TargetHashCode);

            return logMessage;
        }

        class CorrelationState
        {
            public int TargetHashCode { get; set; }

            public object InnerCorrelationState { get; set; }
        }
    }
}
