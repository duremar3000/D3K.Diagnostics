using System;
using System.Reflection;

namespace D3K.Diagnostics.Common
{
    public class MethodInput
    {
        public MethodInfo Method { get; set; }

        public object[] Arguments { get; set; }

        public object Target { get; set; }

        public Type TargetType { get; set; }
    }
}
