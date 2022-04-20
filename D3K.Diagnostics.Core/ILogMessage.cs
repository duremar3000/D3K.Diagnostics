using System;
using System.Collections.Generic;
using System.Text;

namespace D3K.Diagnostics.Core
{
    public interface ILogMessage : IDictionary<string, object>
    {
        string MessageTemplate { get; } 

        void AddMessageProperty(string propertyName, string propertyValue);

        void AddMessageProperty(string propertyName, object propertyValue);

        void AddMessageProperty(string propertyName, Exception propertyValue);
        
        void AddMessageProperty(string propertyName, int propertyValue);

        void AddMessageProperty(string propertyName, double propertyValue);

        void AddMessageProperty(string propertyName, DateTime propertyValue);
    }
}
