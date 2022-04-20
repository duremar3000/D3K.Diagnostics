using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class CachingLoggerPropertyNamesFactory : ILoggerPropertyNamesFactory
    {
        readonly ConcurrentDictionary<string, string[]> _cache = new ConcurrentDictionary<string, string[]>();

        public ILoggerPropertyNamesFactory Target { get; set; }

        public string[] CreateLoggerPropertyNames(string d3kMessageTemplate)
        {
            var propertyNames = _cache.GetOrAdd(d3kMessageTemplate, Target.CreateLoggerPropertyNames(d3kMessageTemplate));

            return propertyNames;
        }
    }
}
