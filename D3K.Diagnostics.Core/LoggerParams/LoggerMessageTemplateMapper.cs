using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class LoggerMessageTemplateMapper : ILoggerMessageTemplateMapper
    {
        public string ToLoggerMessageTemplate(string d3kMessageTemplate)
        {
            return d3kMessageTemplate.Replace("{{", "{@").Replace("}}", "}");
        }
    }
}
