using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class LoggerParamsFactory : ILoggerParamsFactory
    {
        readonly ILoggerMessageTemplateMapper _loggerMessageTemplateMapper;
        readonly ILoggerPropertyValuesFactory _loggerPropertyValuesFactory;

        public LoggerParamsFactory(
            ILoggerMessageTemplateMapper loggerMessageTemplateMapper,
            ILoggerPropertyValuesFactory loggerPropertyValuesFactory)
        {
            _loggerMessageTemplateMapper = loggerMessageTemplateMapper ?? throw new ArgumentNullException();
            _loggerPropertyValuesFactory = loggerPropertyValuesFactory ?? throw new ArgumentNullException();
        }

        public (string logMessageTemplate, object[] logPropertyValues) CreateLoggerParams(ILogMessage logMessage)
        {
            var logMessageTemplate = _loggerMessageTemplateMapper.ToLoggerMessageTemplate(logMessage.MessageTemplate);

            var logPropertyValues = _loggerPropertyValuesFactory.CreateLoggerPropertyValues(logMessage);

            return (logMessageTemplate, logPropertyValues);
        }
    }
}
