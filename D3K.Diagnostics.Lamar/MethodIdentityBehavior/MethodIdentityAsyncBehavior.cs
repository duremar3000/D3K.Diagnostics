using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Lamar.DynamicInterception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Lamar
{
    public class MethodIdentityAsyncBehavior : IAsyncInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityAsyncBehavior(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        public Task<IMethodInvocationResult> InterceptAsync(IAsyncMethodInvocation methodInvocation)
        {
            var scope = _methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey);

            var res = methodInvocation.InvokeNextAsync();

            res.ContinueWith(t => scope.Dispose());

            return res;
        }
    }
}
