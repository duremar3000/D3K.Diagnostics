using System;
using System.Collections.Generic;
using System.Text;

using LightInject.Interception;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.LightInject
{
    internal static class MethodLogHelper
    {
        public static MethodInput CreateMethodInput(IInvocationInfo invocationInfo)
        {
            return new MethodInput
            {
                Method = invocationInfo.Method,
                Arguments = invocationInfo.Arguments,
                Target = invocationInfo.Proxy.Target,
                TargetType = invocationInfo.Proxy.Target.GetType(),
            };
        }
    }
}
