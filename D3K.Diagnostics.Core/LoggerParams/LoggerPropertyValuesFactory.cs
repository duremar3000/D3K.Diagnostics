using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class LoggerPropertyValuesFactory : ILoggerPropertyValuesFactory
    {
        readonly ILoggerPropertyNamesFactory _loggerPropertyNamesFactory;

        public LoggerPropertyValuesFactory(ILoggerPropertyNamesFactory loggerPropertyNamesFactory)
        {
            _loggerPropertyNamesFactory = loggerPropertyNamesFactory ?? throw new ArgumentNullException();
        }

        public object[] CreateLoggerPropertyValues(ILogMessage logMessage)
        {
            var propertyNames = _loggerPropertyNamesFactory.CreateLoggerPropertyNames(logMessage.MessageTemplate);

            var propertyValues = propertyNames
                .Select(i => new { HasValue = logMessage.TryGetValue(i, out object value), Value = value })
                .Where(i => i.HasValue)
                .Select(i => i.Value).ToArray();

            return propertyValues;
        }
    }
}
