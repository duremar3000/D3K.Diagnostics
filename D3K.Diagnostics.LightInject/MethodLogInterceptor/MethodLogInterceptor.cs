using System;
using System.Collections.Generic;
using System.Text;

using LightInject.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.LightInject
{
    using static MethodLogHelper;

    public class MethodLogInterceptor : IInterceptor
    {
        readonly ILogger _logger;
        readonly IMethodLogMessageFactory _methodLogMessageFactory;

        public MethodLogInterceptor(
            ILogger logger,
            IMethodLogMessageFactory methodLogMessageFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _methodLogMessageFactory = methodLogMessageFactory ?? throw new ArgumentNullException();
        }

        public object Invoke(IInvocationInfo invocationInfo)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(invocationInfo);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                var returnValue = invocationInfo.Proceed();

                // AFTER the target method execution 
                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(new MethodReturn { ReturnValue = returnValue}, correlationState);

                _logger.Info(msgOutputLog);

                return returnValue;
            }
            catch (Exception exp)
            {
                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(new MethodReturn { Exception = exp }, correlationState);

                _logger.Error(msgOutputLog);

                throw;
            }
        }
    }
}
