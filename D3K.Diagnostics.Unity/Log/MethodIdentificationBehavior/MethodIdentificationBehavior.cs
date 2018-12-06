using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity.Log
{
    public class MethodIdentificationBehavior : IInterceptionBehavior
    {
        readonly string _mehodIdentityKey;
        readonly IThreadContext _threadContext;

        public MethodIdentificationBehavior(
            string mehodIdentityKey, 
            IThreadContext threadContext)
        {
            if (string.IsNullOrEmpty(mehodIdentityKey))
                throw new ArgumentException();

            _mehodIdentityKey = mehodIdentityKey;
            _threadContext = threadContext ?? throw new ArgumentNullException();
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
            var currMethodId = Guid.NewGuid().ToString().GetHashCode().ToString("x").PadLeft(8, 'x');

            try
            {
                AddMethodId(currMethodId);

                return getNext().Invoke(input, getNext);
            }
            finally
            {
                DelMethodId(currMethodId);
            }
        }

        private void AddMethodId(string currMethodId)
        {
            var fullMethodId = _threadContext[_mehodIdentityKey]?.ToString();

            if (string.IsNullOrEmpty(fullMethodId))
            {
                fullMethodId = currMethodId;
            }
            else
            {
                fullMethodId = $"{fullMethodId}+{currMethodId}";
            }

            _threadContext[_mehodIdentityKey] = fullMethodId;
        }

        private void DelMethodId(string currPid)
        {
            var fullMethodId = _threadContext[_mehodIdentityKey]?.ToString();

            if (!string.IsNullOrEmpty(fullMethodId))
            {
                if (fullMethodId.StartsWith(currPid))
                {
                    _threadContext[_mehodIdentityKey] = null;
                }
                else
                {
                    _threadContext[_mehodIdentityKey] = fullMethodId.Replace($"+{currPid}", "");
                }
            }
        }
    }
}
