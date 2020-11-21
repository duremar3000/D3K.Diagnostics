using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

using Unity.Interception.PolicyInjection.Pipeline;

using D3K.Diagnostics.Common;

namespace D3K.Diagnostics.Unity
{
    internal static class MethodLogHelper
    {
        public static MethodInput CreateMethodInput(IMethodInvocation input)
        {
            return new MethodInput
            {
                Method = input.MethodBase as MethodInfo,
                Arguments = input.Arguments.Cast<object>().ToArray(),
                Target = input.Target,
                TargetType = input.Target.GetType(),
            };
        }
    }
}
