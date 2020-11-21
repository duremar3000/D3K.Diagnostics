using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;

namespace D3K.Diagnostics.Common
{
    public class LogMessage : ILogMessage
    {
        readonly string _messageTemplate;
        readonly StringDictionary _properties = new StringDictionary();

        public LogMessage(string messageTemplate)
        {
            _messageTemplate = messageTemplate ?? throw new ArgumentNullException();
        }

        public void AddMessageProperty(string propertyName, object propertyValue)
        {
            _properties[propertyName] = propertyValue.ToString();
        }

        public override string ToString()
        {
            var res = Regex.Replace(_messageTemplate, @"{{(?<propertyName>[^{]+)}}", match => _properties[match.Groups["propertyName"].Value]);

            return res;
        }
    }
}
