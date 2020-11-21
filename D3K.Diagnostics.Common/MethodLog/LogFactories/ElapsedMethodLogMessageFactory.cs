using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class ElapsedMethodLogMessageFactory : IMethodLogMessageFactory
    {
        public IMethodLogMessageFactory Target { get; set; }

        public (ILogMessage logMessage, object correlationState) CreateInputMethodLogMessage(MethodInput methodInput)
        {
            var sw = new Stopwatch();

            var (logMessage, correlationState) = Target.CreateInputMethodLogMessage(methodInput);

            sw.Start();

            return (logMessage, correlationState: new CorrelationState { Stopwatch = sw, InnerCorrelationState = correlationState });
        }

        public ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, CorrelationState correlationState)
        {
            correlationState.Stopwatch.Stop();

            var logMessage = Target.CreateOutputMethodLogMessage(methodReturn, correlationState.InnerCorrelationState);

            logMessage.AddMessageProperty("Elapsed", correlationState.Stopwatch.Elapsed.TotalMilliseconds);

            return logMessage;
        }

        class CorrelationState
        {
            public Stopwatch Stopwatch { get; set; }

            public object InnerCorrelationState { get; set; }
        }
    }
}
