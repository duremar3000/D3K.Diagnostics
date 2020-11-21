using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    public class MethodIdentityAsyncInterceptionBehavior : AsyncInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityAsyncInterceptionBehavior(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        protected override Task InvokeAsync(IMethodInvocation input, Task proceed)
        {
            var scope = _methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey);

            proceed.ContinueWith(t => scope.Dispose());

            return proceed;
        }

        protected override Task<T> InvokeAsync<T>(IMethodInvocation input, Task<T> proceed)
        {
            var scope = _methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey);

            proceed.ContinueWith(t => scope.Dispose());

            return proceed;
        }
    }
}
