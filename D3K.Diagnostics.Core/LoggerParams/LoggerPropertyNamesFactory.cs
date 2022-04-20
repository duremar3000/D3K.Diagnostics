using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace D3K.Diagnostics.Core
{
    public class LoggerPropertyNamesFactory : ILoggerPropertyNamesFactory
    {
        public string[] CreateLoggerPropertyNames(string d3kMessageTemplate)
        {
            var matchItems = Regex.Matches(d3kMessageTemplate, @"{{(?<propertyName>[^{]+)}}");

            var propertyNames = matchItems.Cast<Match>()
                .Select(i => i.Groups["propertyName"])
                .Select(i => new { i.Index, i.Value })
                .OrderBy(i => i.Index)
                .Select(i => i.Value).ToArray();

            return propertyNames;
        }
    }
}
