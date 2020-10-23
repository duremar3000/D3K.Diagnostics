using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Unity
{
    public class MethodIdentityBehavior : IInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly ILoggingContext _loggingContext;

        public MethodIdentityBehavior(
            string mehodIdentityKey,
            ILoggingContext loggingContext)
        {
            if (string.IsNullOrEmpty(mehodIdentityKey))
                throw new ArgumentException();

            _mehodIdentityKey = mehodIdentityKey;
            _loggingContext = loggingContext ?? throw new ArgumentNullException();
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
            var textMethodIdentificator = _loggingContext[_mehodIdentityKey]?.ToString();

            var currMethodIdentificator = MethodIdentificator.Create(textMethodIdentificator);
            var nextMethodIdentificator = currMethodIdentificator.GetNext();
           
            try
            {
                _loggingContext[_mehodIdentityKey] = nextMethodIdentificator.ToString();

                return getNext().Invoke(input, getNext);
            }
            finally
            {
                _loggingContext[_mehodIdentityKey] = currMethodIdentificator.ToString();
            }
        }
    }
}
