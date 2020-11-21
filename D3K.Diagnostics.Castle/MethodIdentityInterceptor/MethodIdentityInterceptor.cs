using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Castle.DynamicProxy;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Castle
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

        public void Intercept(IInvocation invocation)
        {
            using (_methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey))
            {
                invocation.Proceed();
            }
        }
    }
}
