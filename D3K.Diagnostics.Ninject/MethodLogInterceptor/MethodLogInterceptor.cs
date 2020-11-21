using System;
using System.Collections.Generic;
using System.Text;

using Ninject.Extensions.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Ninject
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

        public void Intercept(IInvocation invocation)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(invocation);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                invocation.Proceed();

                // AFTER the target method execution 
                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(new MethodReturn { ReturnValue = invocation.ReturnValue}, correlationState);

                _logger.Info(msgOutputLog);
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
