using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Unity.Interception.PolicyInjection.Pipeline;

namespace D3K.Diagnostics.Unity
{
    internal static class MethodLogHelper
    {
        public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public static Type GetTargetType(IMethodInvocation input)
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

        public static string ToShortName(Type type)
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

        public static object[] ToArgs(this ILogValueMapper logValueMapper, IMethodInvocation input)
        {
            if (input == null)
                throw new ArgumentNullException();

            var args = input.MethodBase.GetParameters()
                .Select(i => new
                {
                    i.Name,
                    Value = logValueMapper.Map(input.Arguments[i.Name])
                });

            return args.ToArray();
        }

        public static string ToJson(this ILogValueMapper logValueMapper, object traceValue)
        {
            var targetTraceValue = logValueMapper.Map(traceValue);

            var json = JsonConvert.SerializeObject(targetTraceValue, JsonSettings);

            return json;
        }
    }
}
