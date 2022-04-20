using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public interface ILoggerMessageTemplateMapper
    {
        string ToLoggerMessageTemplate(string d3kMessageTemplate);
    }
}
