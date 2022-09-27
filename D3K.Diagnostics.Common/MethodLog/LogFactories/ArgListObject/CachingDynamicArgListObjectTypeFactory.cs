using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class CachingDynamicArgListObjectTypeFactory : IDynamicArgListObjectTypeFactory
    {
        readonly ConcurrentDictionary<MethodInfo, Type> _cache = new ConcurrentDictionary<MethodInfo, Type>();

        public IDynamicArgListObjectTypeFactory Target { get; set; }

        public Type CreateDynamicArgListObjectType(MethodInfo methodInfo)
        {
            var type = _cache.GetOrAdd(methodInfo, i => Target.CreateDynamicArgListObjectType(i));

            return type;
        }
    }
}
