using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace D3K.Diagnostics.Common
{
    using static MethodLogMessageFactoryHelper;

    public class MethodLogMessageFactory : IMethodLogMessageFactory
    {
        readonly ILogMessageSettings _logMessageSettings;
        readonly ILogValueMapper _logValueMapper;

        public MethodLogMessageFactory(
            ILogMessageSettings logMessageSettings,
            ILogValueMapper logValueMapper)
        {
            _logMessageSettings = logMessageSettings ?? throw new ArgumentNullException();
            _logValueMapper = logValueMapper ?? throw new ArgumentNullException();
        }

        public (ILogMessage logMessage, object correlationState) CreateInputMethodLogMessage(MethodInput methodInput)
        {
            var targetType = GetTargetType(methodInput);

            var logMessage = new LogMessage(_logMessageSettings.InputLogMessageTemplate);

            var className = ToShortName(targetType);
            logMessage.AddMessageProperty("ClassName", className);

            var methodName = methodInput.Method.Name;
            logMessage.AddMessageProperty("MethodName", methodName);
            
            var inputArgs = JsonConvert.SerializeObject(_logValueMapper.ToArgs(methodInput), JsonSettings);
            logMessage.AddMessageProperty("InputArgs", inputArgs);

            return (logMessage, correlationState: new CorrelationState { Input = methodInput, ClassName = className, MethodName = methodName });
        }

        public ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, CorrelationState correlationState)
        {
            if (methodReturn.Exception == null)
            {
                var logMessage = new LogMessage(_logMessageSettings.OutputLogMessageTemplate);

                logMessage.AddMessageProperty("ClassName", correlationState.ClassName);

                logMessage.AddMessageProperty("MethodName", correlationState.MethodName);

                if (methodReturn.ReturnValue == null)
                {
                    if (correlationState.Input?.Method?.ReturnType == typeof(void))
                    {
                        logMessage.AddMessageProperty("ReturnValue", "Void");

                        return logMessage;
                    }
                }

                var returnValue = _logValueMapper.ToJson(methodReturn.ReturnValue);

                logMessage.AddMessageProperty("ReturnValue", returnValue);

                return logMessage;
            }
            else
            {
                var logMessage = new LogMessage(_logMessageSettings.ErrorLogMessageTemplate);

                logMessage.AddMessageProperty("ClassName", correlationState.ClassName);

                logMessage.AddMessageProperty("MethodName", correlationState.MethodName);

                logMessage.AddMessageProperty("Exception", methodReturn.Exception);

                return logMessage;
            }
        }

        class CorrelationState
        {
            public MethodInput Input { get; set; }

            public string ClassName { get; set; }

            public string MethodName { get; set; }
        }
    }
}
