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

        public void Attach(ILogObserver observer)
        {
            Log += observer.Log;
        }

        public void Detach(ILogObserver observer)
        {
            Log -= observer.Log;
        }

        public void Debug(string message)
        {
            if (_isDebug)
            {
                Debug(message, null);
            }
        }

        public void Debug(string message, Exception exception)
        {
            if (_isDebug)
            {
                OnLog(new LogEventArgs(LogSeverity.Debug, message, exception, DateTime.Now));
            }
        }

        public void Info(string message)
        {
            if (_isInfo)
                Info(message, null);
        }

        public void Info(string message, Exception exception)
        {
            if (_isInfo)
            {
                OnLog(new LogEventArgs(LogSeverity.Info, message, exception, DateTime.Now));
            }
        }

        public void Warning(string message)
        {
            if (_isWarning)
                Warning(message, null);
        }

        public void Warning(string message, Exception exception)
        {
            if (_isWarning)
            {
                OnLog(new LogEventArgs(LogSeverity.Warning, message, exception, DateTime.Now));
            }
        }

        public void Error(string message)
        {
            if (_isError)
                Error(message, null);
        }

        public void Error(string message, Exception exception)
        {
            if (_isError)
            {
                OnLog(new LogEventArgs(LogSeverity.Error, message, exception, DateTime.Now));
            }
        }

        public void Fatal(string message)
        {
            if (_isFatal)
                Fatal(message, null);
        }

        public void Fatal(string message, Exception exception)
        {
            if (_isFatal)
            {
                OnLog(new LogEventArgs(LogSeverity.Fatal, message, exception, DateTime.Now));
            }
        }

        private void OnLog(LogEventArgs e)
        {
            Log?.Invoke(this, e);
        }

        #endregion
    }
}
