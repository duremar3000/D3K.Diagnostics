using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    public class ElapsedMethodLogMessageFactory : IMethodLogMessageFactory
    {
        public IMethodLogMessageFactory Target { get; set; }

        public (string message, object correlationState) CreateInputMethodLogMessage(IMethodInvocation input)
        {
            var sw = new Stopwatch();

            var (message, correlationState) = Target.CreateInputMethodLogMessage(input);

            sw.Start();

            return (message, correlationState: new CorrelationState { Stopwatch = sw, InnerCorrelationState = correlationState });
        }

        public string CreateOutputMethodLogMessage(IMethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private string CreateOutputMethodLogMessage(IMethodReturn methodReturn, CorrelationState correlationState)
        {
            correlationState.Stopwatch.Stop();

            var msg = Target.CreateOutputMethodLogMessage(methodReturn, correlationState.InnerCorrelationState);

            var res = msg.Replace("<< ", $"<< Elapsed: {correlationState.Stopwatch.Elapsed.TotalMilliseconds}ms. ");

            return res;
        }

        class CorrelationState
        {
            public Stopwatch Stopwatch { get; set; }

            public object InnerCorrelationState { get; set; }
        }
    }
}
