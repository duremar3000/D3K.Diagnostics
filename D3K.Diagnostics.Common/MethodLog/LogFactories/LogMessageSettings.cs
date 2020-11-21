using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class LogMessageSettings : ILogMessageSettings
    {
        public string InputLogMessageTemplate { get; set; } = ">>{{ClassName}}.{{MethodName}}>> InputArgs: {{InputArgs}}";

        public string OutputLogMessageTemplate { get; set; } = "<<{{ClassName}}.{{MethodName}}<< Elapsed: {{Elapsed}}ms. ReturnValue: {{ReturnValue}}";

        public string ErrorLogMessageTemplate { get; set; } = "<<{{ClassName}}.{{MethodName}}<< Elapsed: {{Elapsed}}ms. Exception: {{Exception}}";
    }
}
