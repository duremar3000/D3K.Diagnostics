using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class CachingNLogMessageTemplateMapper: ILoggerMessageTemplateMapper
    {
        readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();

        public ILoggerMessageTemplateMapper Target { get; set; }

        public string ToLoggerMessageTemplate(string d3kMessageTemplate)
        {
            var propertyNames = _cache.GetOrAdd(d3kMessageTemplate, Target.ToLoggerMessageTemplate(d3kMessageTemplate));

            return propertyNames;
        }
    }
}
