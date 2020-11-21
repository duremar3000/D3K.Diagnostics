using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Lamar.DynamicInterception;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    using static MethodLogHelper;

    public class MethodLogBehavior : ISyncInterceptionBehavior
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

        public IMethodInvocationResult Intercept(ISyncMethodInvocation methodInvocation)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(methodInvocation);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            // Yield to the next module in the pipeline
            var methodReturn = methodInvocation.InvokeNext();

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
