using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    public class MethodIdentityInterceptionBehavior : IInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly IMethodIdentityProvider _methodIdentityProvider;

        public MethodIdentityInterceptionBehavior(string mehodIdentityKey, IMethodIdentityProvider methodIdentityProvider)
        {
            _mehodIdentityKey = mehodIdentityKey ?? throw new ArgumentNullException();
            _methodIdentityProvider = methodIdentityProvider ?? throw new ArgumentNullException();
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            using (_methodIdentityProvider.BeginNextMethodIdentityScope(_mehodIdentityKey))
            {
                return getNext().Invoke(input, getNext);
            }
        }
    }
}
