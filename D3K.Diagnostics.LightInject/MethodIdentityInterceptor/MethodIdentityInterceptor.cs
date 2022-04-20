using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LightInject.Interception;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.LightInject
{
    public class MethodIdentityInterceptor : IInterceptor
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityInterceptor(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        public object Invoke(IInvocationInfo invocationInfo)
        {
            using (_methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey))
            {
                return invocationInfo.Proceed();
            }
        }
    }
}
