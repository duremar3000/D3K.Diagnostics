using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Unity
{
    public class MethodLogBehavior : IInterceptionBehavior
    {
        readonly ILogger _logger;
        readonly IMethodLogMessageFactory _methodLogMessageFactory;

        public MethodLogBehavior(
            ILogger logger,
            IMethodLogMessageFactory methodLogMessageFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _methodLogMessageFactory = methodLogMessageFactory ?? throw new ArgumentNullException();
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            // BEFORE the target method execution
            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(input);

            _logger.Info(msgInputLog);

            // Yield to the next module in the pipeline
            var methodReturn = getNext().Invoke(input, getNext);

            // AFTER the target method execution 
            var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

            if (methodReturn.Exception == null)
            {
                _logger.Info(msgOutputLog);
            }
            else
            {
                _logger.Error(msgOutputLog);
            }

            return methodReturn;
        }
    }
}
