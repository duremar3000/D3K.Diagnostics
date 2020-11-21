using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Ninject.Extensions.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Ninject
{
    public class MethodIdentityAsyncInterceptor : AsyncInterceptor
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityAsyncInterceptor(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        protected override Task InterceptAsync(IInvocation invocation, Task proceed)
        {
            var scope = _methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey);

            proceed.ContinueWith(t => scope.Dispose());

            return proceed;
        }

        protected override Task<T> InterceptAsync<T>(IInvocation invocation, Task<T> proceed)
        {
            var scope = _methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey);

            proceed.ContinueWith(t => scope.Dispose());

            return proceed;
        }
    }
}
