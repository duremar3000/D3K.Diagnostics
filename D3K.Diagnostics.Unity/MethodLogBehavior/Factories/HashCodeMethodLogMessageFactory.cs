using System;
using System.Collections.Generic;
using System.Text;

using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    public class HashCodeMethodLogMessageFactory : IMethodLogMessageFactory
    {
        public IMethodLogMessageFactory Target { get; set; }

        public (string message, object correlationState) CreateInputMethodLogMessage(IMethodInvocation input)
        {
            var (message, correlationState) = Target.CreateInputMethodLogMessage(input);

            var targetHashCode = input.Target.GetHashCode();

            message = DecorateMessageByTargetHashCode(message, targetHashCode, ">>");

            return (message, correlationState: new CorrelationState { TargetHashCode = targetHashCode, InnerCorrelationState = correlationState });
        }

        public string CreateOutputMethodLogMessage(IMethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private string CreateOutputMethodLogMessage(IMethodReturn methodReturn, CorrelationState correlationState)
        {
            var message = Target.CreateOutputMethodLogMessage(methodReturn, correlationState.InnerCorrelationState);

            message = DecorateMessageByTargetHashCode(message, correlationState.TargetHashCode, "<<");

            return message;
        }

        private string DecorateMessageByTargetHashCode(string message, int targetHashCode, string direction)
        {
            var msg = message.Split(new[] { direction }, StringSplitOptions.RemoveEmptyEntries);

            msg[0] = $"{direction}{msg[0].Replace(",", $"+{targetHashCode},")}{direction}";

            var res = string.Concat(msg);

            return res;
        }

        class CorrelationState
        {
            public int TargetHashCode { get; set; }

            public object InnerCorrelationState { get; set; }
        }
    }
}
