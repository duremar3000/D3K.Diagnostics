using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

using Unity.Interception.InterceptionBehaviors;
using Unity.Interception.PolicyInjection.Pipeline;

using Newtonsoft.Json;

using D3K.Diagnostics.Core.Log;

namespace D3K.Diagnostics.Unity.Log
{
    public class MethodInvokeLoggingBehavior : IInterceptionBehavior
    {
        readonly ILogger _logger;
        readonly ILogValueMapper _logValueMapper;

        readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public MethodInvokeLoggingBehavior(
            ILogger logger,
            ILogValueMapper logValueMapper)
        {
            _logger = logger ?? throw new ArgumentNullException();
            _logValueMapper = logValueMapper ?? throw new ArgumentNullException();
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
            var targetType = GetTargetType(input);

            string methodInfo = $"{ToShortName(targetType)}, {input.MethodBase.Name}";           

            // BEFORE the target method execution
            var inputJson = JsonConvert.SerializeObject(ToArgs(input), _jsonSettings);
            _logger.Debug($">>{methodInfo}>> InputArgs: {inputJson}");

            var sw = new Stopwatch();
            sw.Start();

            // Yield to the next module in the pipeline
            var methodReturn = getNext().Invoke(input, getNext);

            // AFTER the target method execution 
            sw.Stop();

            var sb = new StringBuilder();
            sb.AppendFormat("<<{0}<< Elapsed: {1}ms.", methodInfo, sw.Elapsed.TotalMilliseconds).Append(' ');

            if (methodReturn.Exception == null)
            {
                var mi = input.MethodBase as MethodInfo;
                if (mi?.ReturnType == typeof(void))
                {
                    sb.AppendFormat("ReturnValue: Void");
                }
                else
                {
                    var returnJson = ToJson(methodReturn.ReturnValue);

                    sb.AppendFormat("ReturnValue: {0}", returnJson);
                }

                _logger.Info(sb.ToString());
            }
            else
            {
                sb.AppendFormat("Exception: {0}", methodReturn.Exception);

                _logger.Error(sb.ToString());
            }

            return methodReturn;
        }

        private Type GetTargetType(IMethodInvocation input)
        {
            Type targetType;
            if (input.MethodBase.DeclaringType.IsAbstract)
            {
                targetType = input.Target.GetType();
            }
            else
            {
                targetType = input.MethodBase.DeclaringType;
            }

            return targetType;
        }

        private string ToShortName(Type type)
        {
            var res = type.ToString();
            var namespaces = new List<string> { type.Namespace };

            if (type.IsGenericType)
            {
                var genericTypeArguments = type.GenericTypeArguments;

                for (int num = 0; num < genericTypeArguments.Length; num++)
                {
                    namespaces.Add(genericTypeArguments[num].Namespace);
                }
            }

            var items = namespaces.OrderByDescending(i => i.Count());

            foreach (string item in items)
            {
                res = res.Replace(string.Format("{0}.", item), "");
            }

            return res;
        }

        private object[] ToArgs(IMethodInvocation input)
        {
            if (input == null)
                throw new ArgumentNullException();

            var args = input.MethodBase.GetParameters()
                .Select(i => new
                {
                    i.Name,
                    Value = _logValueMapper.Map(input.Arguments[i.Name])
                });

            return args.ToArray();
        }

        private string ToJson(object traceValue)
        {
            var targetTraceValue = _logValueMapper.Map(traceValue);

            var json = JsonConvert.SerializeObject(targetTraceValue, _jsonSettings);

            return json;
        }
    }
}
