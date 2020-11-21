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

        public void Debug(object message)
        {
            if (_isDebug)
            {
                Debug(message, null);
            }
        }

        public void Debug(object message, Exception exception)
        {
            if (_isDebug)
            {
                OnLog(new LogEventArgs(LogSeverity.Debug, message.ToString(), exception, DateTime.Now));
            }
        }

        public void Info(object message)
        {
            if (_isInfo)
                Info(message, null);
        }

        public void Info(object message, Exception exception)
        {
            if (_isInfo)
            {
                OnLog(new LogEventArgs(LogSeverity.Info, message.ToString(), exception, DateTime.Now));
            }
        }

        public void Warning(object message)
        {
            if (_isWarning)
                Warning(message, null);
        }

        public void Warning(object message, Exception exception)
        {
            if (_isWarning)
            {
                OnLog(new LogEventArgs(LogSeverity.Warning, message.ToString(), exception, DateTime.Now));
            }
        }

        public void Error(object message)
        {
            if (_isError)
                Error(message, null);
        }

        public void Error(object message, Exception exception)
        {
            if (_isError)
            {
                OnLog(new LogEventArgs(LogSeverity.Error, message.ToString(), exception, DateTime.Now));
            }
        }

        public void Fatal(object message)
        {
            if (_isFatal)
                Fatal(message, null);
        }

        public void Fatal(object message, Exception exception)
        {
            if (_isFatal)
            {
                OnLog(new LogEventArgs(LogSeverity.Fatal, message.ToString(), exception, DateTime.Now));
            }
        }

        private void OnLog(LogEventArgs e)
        {
            Log?.Invoke(this, e);
        }

        #endregion
    }
}
