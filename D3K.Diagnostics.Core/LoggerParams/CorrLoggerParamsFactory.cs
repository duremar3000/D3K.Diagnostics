using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public class CorrLoggerParamsFactory : ILoggerParamsFactory
    {
        public ILoggerParamsFactory Target { get; set; }

        public (string logMessageTemplate, object[] logPropertyValues) CreateLoggerParams(ILogMessage logMessage)
        {
            var (logMessageTemplate, logPropertyValues) = Target.CreateLoggerParams(logMessage);

            logMessageTemplate = CorrMessageTemplate(logMessageTemplate, logMessage);

            return (logMessageTemplate, logPropertyValues);
        }

        private string CorrMessageTemplate(string logMessageTemplate, IDictionary<string, object> properties)
        {
            if (properties.ContainsKey("InputArgs"))
            {
                logMessageTemplate = logMessageTemplate.Replace(
                    "{@InputArgs}",
                    $"{{@{properties["MethodDeclaringTypeName"]}_{properties["MethodName"]}_InputArgs}}");
            }

            if (properties.ContainsKey("ReturnValue"))
            {
                logMessageTemplate = logMessageTemplate.Replace(
                    "{@ReturnValue}",
                    $"{{@{properties["MethodDeclaringTypeName"]}_{properties["MethodName"]}_ReturnValue}}");
            }

            return logMessageTemplate;
        }
    }
}
