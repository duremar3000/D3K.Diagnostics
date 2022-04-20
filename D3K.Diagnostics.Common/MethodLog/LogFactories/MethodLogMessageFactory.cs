using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.Common
{
    using static MethodLogMessageFactoryHelper;

    public class MethodLogMessageFactory : IMethodLogMessageFactory
    {
        readonly ILogMessageSettings _logMessageSettings;
        readonly ILogValueMapper _logValueMapper;
        readonly ILogMessageFactory _logMessageFactory;

        public MethodLogMessageFactory(
            ILogMessageSettings logMessageSettings,
            ILogValueMapper logValueMapper,
            ILogMessageFactory logMessageFactory)
        {
            _logMessageSettings = logMessageSettings ?? throw new ArgumentNullException();
            _logValueMapper = logValueMapper ?? throw new ArgumentNullException();
            _logMessageFactory = logMessageFactory ?? throw new ArgumentNullException();
        }

        public (ILogMessage logMessage, object correlationState) CreateInputMethodLogMessage(MethodInput methodInput)
        {
            var logMessage = _logMessageFactory.CreateLogMessage(_logMessageSettings.InputLogMessageTemplate);

            var methodDeclaringType = methodInput.Method.DeclaringType;
            var methodDeclaringTypeName = ToShortName(methodDeclaringType);
            logMessage.AddMessageProperty("MethodDeclaringTypeName", methodDeclaringTypeName);

            var targetType = GetTargetType(methodInput);
            var className = ToShortName(targetType);
            logMessage.AddMessageProperty("ClassName", className);

            var methodName = methodInput.Method.Name;
            logMessage.AddMessageProperty("MethodName", methodName);

            var inputArgs = _logValueMapper.ToArgs(methodInput);
            logMessage.AddMessageProperty("InputArgs", inputArgs);

            return (logMessage, correlationState: new CorrelationState { Input = methodInput, MethodDeclaringTypeName = methodDeclaringTypeName, ClassName = className, MethodName = methodName });
        }

        public ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private ILogMessage CreateOutputMethodLogMessage(MethodReturn methodReturn, CorrelationState correlationState)
        {
            if (methodReturn.Exception == null)
            {
                var logMessage = _logMessageFactory.CreateLogMessage(_logMessageSettings.OutputLogMessageTemplate);

                logMessage.AddMessageProperty("MethodDeclaringTypeName", correlationState.MethodDeclaringTypeName);

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

                logMessage.AddMessageProperty("ReturnValue", methodReturn.ReturnValue);

                return logMessage;
            }
            else
            {
                var logMessage = _logMessageFactory.CreateLogMessage(_logMessageSettings.ErrorLogMessageTemplate);

                logMessage.AddMessageProperty("MethodDeclaringTypeName", correlationState.MethodDeclaringTypeName);

                logMessage.AddMessageProperty("ClassName", correlationState.ClassName);

                logMessage.AddMessageProperty("MethodName", correlationState.MethodName);

                logMessage.AddMessageProperty("Exception", methodReturn.Exception);

                return logMessage;
            }
        }

        class CorrelationState
        {
            public MethodInput Input { get; set; }

            public string MethodDeclaringTypeName { get; set; }

            public string ClassName { get; set; }

            public string MethodName { get; set; }
        }
    }
}
