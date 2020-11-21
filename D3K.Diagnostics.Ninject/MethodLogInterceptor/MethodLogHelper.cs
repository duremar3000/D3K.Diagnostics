using System;
using System.Collections.Generic;
using System.Text;

using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Request;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Ninject
{
    internal static class MethodLogHelper
    {
        public static MethodInput CreateMethodInput(IInvocation invocation)
        {
            return CreateMethodInput(invocation.Request);
        }

        public static MethodInput CreateMethodInput(IProxyRequest proxyRequest)
        {
            return new MethodInput
            {
                Method = proxyRequest.Method,
                Arguments = proxyRequest.Arguments,
                Target = proxyRequest.Target,
                TargetType = proxyRequest.Target.GetType(),
            };
        }
    }
}
