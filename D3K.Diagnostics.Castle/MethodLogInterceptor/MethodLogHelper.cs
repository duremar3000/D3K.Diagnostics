using System;
using System.Collections.Generic;
using System.Text;

using Castle.DynamicProxy;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Castle
{
    internal static class MethodLogHelper
    {
        public static MethodInput CreateMethodInput(IInvocation invocation)
        {
            return new MethodInput
            {
                Method = invocation.Method,
                Arguments = invocation.Arguments,
                Target = invocation.InvocationTarget,
                TargetType = invocation.TargetType,
            };
        }
    }
}
