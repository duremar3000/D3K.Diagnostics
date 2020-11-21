using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    using static MethodLogHelper;

    public class MethodLogInterceptionBehavior : IInterceptionBehavior
    {
        readonly ILogger _logger;
        readonly IMethodLogMessageFactory _methodLogMessageFactory;

        public MethodLogInterceptionBehavior(
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
            var methodInput = CreateMethodInput(input);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            // Yield to the next module in the pipeline
            var methodReturn = getNext().Invoke(input, getNext);

            // AFTER the target method execution 
            var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(new MethodReturn { ReturnValue = methodReturn.ReturnValue, Exception = methodReturn.Exception }, correlationState);

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
