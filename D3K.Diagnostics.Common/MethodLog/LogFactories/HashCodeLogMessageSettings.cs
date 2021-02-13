using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public class HashCodeLogMessageSettings : ILogMessageSettings
    {
        public string InputLogMessageTemplate { get; set; } = ">>{{ClassName}}.{{MethodName}}+{{HashCode}}>> InputArgs: {{InputArgs}}";

        public string OutputLogMessageTemplate { get; set; } = "<<{{ClassName}}.{{MethodName}}+{{HashCode}}<< Elapsed: {{Elapsed}}ms. ReturnValue: {{ReturnValue}}";

        public string ErrorLogMessageTemplate { get; set; } = "<<{{ClassName}}.{{MethodName}}+{{HashCode}}<< Elapsed: {{Elapsed}}ms. Exception: {{Exception}}";
    }
}
