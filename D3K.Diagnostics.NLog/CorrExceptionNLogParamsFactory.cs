using System;
using System.Collections.Generic;
using System.Text;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.NLogExtensions
{
    public class CorrExceptionNLogParamsFactory : ILoggerParamsFactory
    {
        public ILoggerParamsFactory Target { get; set; }

        public (string logMessageTemplate, object[] logPropertyValues) CreateLoggerParams(ILogMessage logMessage)
        {
            if (logMessage.ContainsKey("Exception"))
            {
                logMessage = CorrLogMessage(logMessage);
            }

            return Target.CreateLoggerParams(logMessage);
        }

        private ILogMessage CorrLogMessage(ILogMessage logMessage)
        {
            var res = new LogMessage(logMessage);

            res.MessageTemplate = logMessage.MessageTemplate.Replace("{{Exception}}", "");

            res.Remove("Exception");

            return res;
        }

        class LogMessage : Dictionary<string, object>, ILogMessage
        {
            public LogMessage(ILogMessage logMessage)
            {
                MessageTemplate = logMessage.MessageTemplate;

                foreach (var prop in logMessage)
                {
                    this[prop.Key] = prop.Value;
                }
            }

            public string MessageTemplate { get; set; }

            public void AddMessageProperty(string propertyName, string propertyValue)
            {
                Add(propertyName, propertyValue);
            }

            public void AddMessageProperty(string propertyName, object propertyValue)
            {
                Add(propertyName, propertyValue);
            }

            public void AddMessageProperty(string propertyName, Exception propertyValue)
            {
                Add(propertyName, propertyValue);
            }

            public void AddMessageProperty(string propertyName, int propertyValue)
            {
                Add(propertyName, propertyValue);
            }

            public void AddMessageProperty(string propertyName, double propertyValue)
            {
                Add(propertyName, propertyValue);
            }

            public void AddMessageProperty(string propertyName, DateTime propertyValue)
            {
                Add(propertyName, propertyValue);
            }
        }
    }
}
