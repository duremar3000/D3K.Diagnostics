using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3K.Diagnostics.Core
{
    public sealed class Logger : ILogger
    {
        #region Fields

        LogSeverity _severity;

        bool _isDebug;
        bool _isInfo;
        bool _isWarning;
        bool _isError;
        bool _isFatal;

        #endregion

        #region Constructors

        public Logger()
        {
            Severity = LogSeverity.Debug;
        }

        #endregion

        #region Events

        public event EventHandler<LogEventArgs> Log;

        #endregion

        #region Properties

        public LogSeverity Severity
        {
            get { return _severity; }
            set
            {
                _severity = value;

                int severity = (int)_severity;

                _isDebug = ((int)LogSeverity.Debug) >= severity ? true : false;
                _isInfo = ((int)LogSeverity.Info) >= severity ? true : false;
                _isWarning = ((int)LogSeverity.Warning) >= severity ? true : false;
                _isError = ((int)LogSeverity.Error) >= severity ? true : false;
                _isFatal = ((int)LogSeverity.Fatal) >= severity ? true : false;
            }
        }

        #endregion

        #region Methods

        public void Attach(ILogListener listener)
        {
            Log += listener.Log;
        }

        public void Detach(ILogListener listener)
        {
            Log -= listener.Log;
        }

        public void Debug(ILogMessage message)
        {
            if (_isDebug)
            {
                OnLog(new LogEventArgs(LogSeverity.Debug, message, DateTime.Now));
            }
        }

        public void Info(ILogMessage message)
        {
            if (_isInfo)
            {
                OnLog(new LogEventArgs(LogSeverity.Info, message, DateTime.Now));
            }
        }

        public void Warning(ILogMessage message)
        {
            if (_isWarning)
            {
                OnLog(new LogEventArgs(LogSeverity.Warning, message, DateTime.Now));
            }
        }

        public void Error(ILogMessage message)
        {
            if (_isError)
            {
                OnLog(new LogEventArgs(LogSeverity.Error, message, DateTime.Now));
            }
        }

        public void Fatal(ILogMessage message)
        {
            if (_isFatal)
            {
                OnLog(new LogEventArgs(LogSeverity.Fatal, message, DateTime.Now));
            }
        }

        private void OnLog(LogEventArgs e)
        {
            Log?.Invoke(this, e);
        }

        #endregion
    }
}
