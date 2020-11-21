using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using D3K.Diagnostics.Core;
using D3K.Diagnostics.Log4netExtensions;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLogListenerFactory : ILogListenerFactory
    {
        readonly static Assembly _entryAssembly = Assembly.GetEntryAssembly();

        public ILogListener CreateLogListener(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            var log = log4net.LogManager.GetLogger(_entryAssembly, loggerName);

            return new Log4netLogListener(log);
        }
    }
}
