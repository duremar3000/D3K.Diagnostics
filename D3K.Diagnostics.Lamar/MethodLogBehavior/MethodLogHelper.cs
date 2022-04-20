using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

using Lamar.DynamicInterception;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Lamar
{
    internal static class MethodLogHelper
    {
        public static MethodInput CreateMethodInput(IMethodInvocation input)
        {
            return new MethodInput
            {
                Method = input.MethodInfo,
                Arguments = input.Arguments.Cast<IArgument>().Select(i => i.Value).ToArray(),
                Target = input.TargetInstance,
                TargetType = input.TargetInstance.GetType(),
            };
        }
    }
}
