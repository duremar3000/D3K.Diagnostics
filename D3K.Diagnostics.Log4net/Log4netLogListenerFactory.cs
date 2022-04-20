using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using D3K.Diagnostics.Core;

using log4net;
using log4net.Config;

namespace D3K.Diagnostics.Log4netExtensions
{
    public class Log4netLogListenerFactory : ILogListenerFactory
    {
        readonly static Assembly _entryAssembly = Assembly.GetEntryAssembly();

        public ILogListener CreateLogListener(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            var loggerRepository = LogManager.GetRepository(_entryAssembly);

            if (!loggerRepository.Configured)
            {
                XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));
            }

            var log = LogManager.GetLogger(_entryAssembly, loggerName);

            return new Log4netLogListener(log);
        }
    }
}
