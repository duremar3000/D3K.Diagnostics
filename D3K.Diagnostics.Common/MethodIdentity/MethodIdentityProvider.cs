using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Common
{
    public class MethodIdentityProvider : IMethodIdentityProvider
    {
        readonly ILogContext _logContext;

        public MethodIdentityProvider(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException();
        }

        public IDisposable BeginNextMethodIdentityScope(string methodIdentityKey)
        {
            var methodIdentity = _logContext.PeekProperty(methodIdentityKey) as MethodIdentity ?? MethodIdentity.Create();

            methodIdentity = methodIdentity.GetNext();

            return _logContext.PushProperty(methodIdentityKey, methodIdentity);
        }
    }
}
