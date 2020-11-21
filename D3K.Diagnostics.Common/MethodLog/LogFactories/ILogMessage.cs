using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Common
{
    public interface ILogMessage
    {
        void AddMessageProperty(string propertyName, object propertyValue);
    }
}
