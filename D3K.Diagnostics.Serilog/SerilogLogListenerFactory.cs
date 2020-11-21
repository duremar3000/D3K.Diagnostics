using System;
using System.Collections.Generic;
using System.Text;

using Serilog;

using D3K.Diagnostics.Core;

namespace D3K.Diagnostics.SerilogExtensions
{
    public class SerilogLogListenerFactory : ILogListenerFactory
    {
        public ILogListener CreateLogListener(string loggerName)
        {
            if (string.IsNullOrEmpty(loggerName))
                throw new ArgumentException();

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .ReadFrom.AppSettings(loggerName, "serilog.config");

            var logger = loggerConfiguration.CreateLogger();

            return new SerilogLogListener(logger);
        }
    }
}
