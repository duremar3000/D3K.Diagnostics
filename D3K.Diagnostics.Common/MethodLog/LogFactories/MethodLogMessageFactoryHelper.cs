using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace D3K.Diagnostics.Common
{
    internal static class MethodLogMessageFactoryHelper
    {
        public static JsonSerializerSettings JsonSettings { get; } = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        public static Type GetTargetType(MethodInput input)
        {
            Type targetType;
            if (input.Method.DeclaringType.IsAbstract)
            {
                targetType = input.TargetType;
            }
            else
            {
                targetType = input.Method.DeclaringType;
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

        public static object[] ToArgs(this ILogValueMapper logValueMapper, MethodInput input)
        {
            if (input == null)
                throw new ArgumentNullException();

            var args = input.Method.GetParameters()
                .Select(i => new
                {
                    i.Name,
                    Value = logValueMapper.Map(input.Arguments[i.Position])
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
