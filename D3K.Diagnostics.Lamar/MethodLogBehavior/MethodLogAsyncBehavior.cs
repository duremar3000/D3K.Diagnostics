using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lamar.DynamicInterception;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    using static MethodLogHelper;

    public class MethodLogAsyncBehavior : IAsyncInterceptionBehavior
    {
        readonly ILogger _logger;
        readonly IMethodLogMessageFactory _methodLogMessageFactory;

        public MethodLogAsyncBehavior(
            ILogger logger,
            IMethodLogMessageFactory methodLogMessageFactory)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _methodLogMessageFactory = methodLogMessageFactory ?? throw new ArgumentNullException();
        }

        public async Task<IMethodInvocationResult> InterceptAsync(IAsyncMethodInvocation methodInvocation)
        {
            // BEFORE the target method execution
            var methodInput = CreateMethodInput(methodInvocation);

            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(methodInput);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                var result = await methodInvocation.InvokeNextAsync().ConfigureAwait(false);

                // AFTER the target method execution 
                var methodReturn = new MethodReturn { ReturnValue = result.ReturnValue };

                if (methodInvocation.ActualReturnType.Name == "Void")
                {
                    methodReturn.ReturnValue = "Void";
                }

                var msgOutputLog = _methodLogMessageFactory.CreateOutputMethodLogMessage(methodReturn, correlationState);

                _logger.Info(msgOutputLog);

                return result;
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