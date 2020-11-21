using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LightInject.Interception;

using D3K.Diagnostics.Common;
using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.LightInject
{
    using static MethodLogHelper;

    public class MethodLogAsyncInterceptor : AsyncInterceptor
    {
        readonly ILogger _logger;
        readonly IMethodLogMessageFactory _methodLogMessageFactory;

        public MethodLogAsyncInterceptor(
            ILogger logger,
            IMethodLogMessageFactory methodLogMessageFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _methodLogMessageFactory = methodLogMessageFactory ?? throw new ArgumentNullException();
        }

        protected override async Task InvokeAsync(IInvocationInfo invocationInfo, Task proceed)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(invocationInfo);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                await proceed.ConfigureAwait(false);

                // AFTER the target method execution 
                var methodReturn = new MethodReturn { ReturnValue = "Void`" };

                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

                _logger.Info(msgOutputLog);
            }
            catch (Exception e)
            {
                var methodReturn = new MethodReturn { Exception = e };

                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

                _logger.Error(msgOutputLog);

                throw;
            }
        }

        protected override async Task<T> InvokeAsync<T>(IInvocationInfo invocationInfo, Task<T> proceed)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(invocationInfo);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                T value = await proceed.ConfigureAwait(false);

                // AFTER the target method execution 
                var methodReturn = new MethodReturn { ReturnValue = value };

                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

                _logger.Info(msgOutputLog);

                return value;
            }
            catch (Exception e)
            {
                var methodReturn = new MethodReturn { Exception = e };

                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

                _logger.Error(msgOutputLog);

                throw;
            }
        }
    }
}
