using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using D3K.Diagnostics.Core;

using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    public class MethodIdentityAsyncBehavior : InterceptionAsyncBehavior
    {
        readonly string _mehodIdentityKey;
        readonly ILoggingContext _loggingLogicalThreadContext;

        public MethodIdentityAsyncBehavior(
            string mehodIdentityKey,
            ILoggingContext loggingContext)
        {
            if (string.IsNullOrEmpty(mehodIdentityKey))
                throw new ArgumentException();

            _mehodIdentityKey = mehodIdentityKey;
            _loggingLogicalThreadContext = loggingContext ?? throw new ArgumentNullException();
        }

        protected override Task InvokeAsync(IMethodInvocation input, Task proceed)
        {
            var textMethodIdentificator = _loggingLogicalThreadContext[_mehodIdentityKey]?.ToString();

            var currMethodIdentificator = MethodIdentificator.Create(textMethodIdentificator);
            var nextMethodIdentificator = currMethodIdentificator.GetNext();

            _loggingLogicalThreadContext[_mehodIdentityKey] = nextMethodIdentificator.ToString();

            return proceed;
        }

        protected override Task<T> InvokeAsync<T>(IMethodInvocation input, Task<T> proceed)
        {
            var textMethodIdentificator = _loggingLogicalThreadContext[_mehodIdentityKey]?.ToString();

            var currMethodIdentificator = MethodIdentificator.Create(textMethodIdentificator);
            var nextMethodIdentificator = currMethodIdentificator.GetNext();

            _loggingLogicalThreadContext[_mehodIdentityKey] = nextMethodIdentificator.ToString();

            return proceed;
        }
    }
}
