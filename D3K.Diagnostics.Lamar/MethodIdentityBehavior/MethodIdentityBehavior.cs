using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lamar.DynamicInterception;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    public class MethodIdentityBehavior : ISyncInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityBehavior(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        public IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation)
        {
            using (_methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey))
            {
                return methodInvocation.InvokeNext();
            }
        }
    }
}
