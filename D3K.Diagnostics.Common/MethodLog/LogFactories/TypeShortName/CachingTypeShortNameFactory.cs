using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class CachingTypeShortNameFactory : ITypeShortNameFactory
    {
        readonly ConcurrentDictionary<Type, string> _cache = new ConcurrentDictionary<Type, string>();

        public ITypeShortNameFactory Target { get; set; }

        public string CreateTypeShortName(Type type)
        {
            var res = _cache.GetOrAdd(type, i => Target.CreateTypeShortName(i));

            return res;
        }
    }
}
