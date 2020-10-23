using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    using static MethodLogHelper;

    public class MethodLogMessageFactory : IMethodLogMessageFactory
    {
        readonly ILogValueMapper _logValueMapper;

        public MethodLogMessageFactory(ILogValueMapper logValueMapper)
        {
            _logValueMapper = logValueMapper ?? throw new ArgumentNullException();
        }

        public (string message, object correlationState) CreateInputMethodLogMessage(IMethodInvocation input)
        {
            var targetType = GetTargetType(input);

            var methodInfo = $"{ToShortName(targetType)}, {input.MethodBase.Name}";

            var inputJson = JsonConvert.SerializeObject(_logValueMapper.ToArgs(input), JsonSettings);
            var msg = $">>{methodInfo}>> InputArgs: {inputJson}";

            return (message: msg, correlationState: new CorrelationState { Input = input, MethodInfo = methodInfo });
        }

        public string CreateOutputMethodLogMessage(IMethodReturn methodReturn, object correlationState)
        {
            return CreateOutputMethodLogMessage(methodReturn, (CorrelationState)correlationState);
        }

        private string CreateOutputMethodLogMessage(IMethodReturn methodReturn, CorrelationState correlationState)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("<<{0}<< ", correlationState.MethodInfo);

            if (methodReturn.Exception == null)
            {
                if (methodReturn.ReturnValue == null)
                {
                    var mi = correlationState.Input.MethodBase as MethodInfo;
                    if (mi?.ReturnType == typeof(void))
                    {
                        sb.AppendFormat("ReturnValue: Void");

                        return sb.ToString();
                    }
                }

                var returnJson = _logValueMapper.ToJson(methodReturn.ReturnValue);

                sb.AppendFormat("ReturnValue: {0}", returnJson);
            }
            else
            {
                sb.AppendFormat("Exception: {0}", methodReturn.Exception);
            }

            return sb.ToString();
        }

        class CorrelationState
        {
            public IMethodInvocation Input { get; set; }

            public string MethodInfo { get; set; }
        }
    }
}
