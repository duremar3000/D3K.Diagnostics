using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Unity
{
    public class MethodLogAsyncBehavior : InterceptionAsyncBehavior
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

        protected override async Task InvokeAsync(IMethodInvocation input, Task proceed)
        {
            // BEFORE the target method execution
            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(input);

            _logger.Info(msgInputLog);

            try
            {
                // Yield to the next module in the pipeline
                await proceed.ConfigureAwait(false);

                // AFTER the target method execution 
                var methodReturn = new MethodReturn { ReturnValue = "Void" };

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

        protected override async Task<T> InvokeAsync<T>(IMethodInvocation input, Task<T> proceed)
        {
            // BEFORE the target method execution
            var (msgInputLog, correlationState) = _methodLogMessageFactory.CreateInputMethodLogMessage(input);

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

        class MethodReturn : IMethodReturn
        {
            public IParameterCollection Outputs => throw new NotImplementedException();

            public object ReturnValue { get; set; }

            public Exception Exception { get; set; }

            public IDictionary<string, object> InvocationContext => throw new NotImplementedException();
        }
    }
}
