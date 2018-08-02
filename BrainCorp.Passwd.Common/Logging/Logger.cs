using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainCorp.Passwd.Common.Logging
{
    public abstract class Logger : ILogger
    {
        #region Public Methods

        public abstract void Write(string message, LogEntrySeverityEnum severity);
        public abstract void Write(string message, LogEntrySeverityEnum severity, Exception exception);

        public virtual void SetLoggableSeverities(LogEntrySeverityEnum severityMask)
        {
            _loggableSeverities = (int)severityMask;
        }

        public virtual bool WillLog(LogEntrySeverityEnum severity)
        {
            try
            {
                return willLog(severity);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        private bool willLog(LogEntrySeverityEnum severity)
        {
            if ((_loggableSeverities & (int)severity) > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper method that will format the log text
        /// and include selected attributes from the exception
        /// object
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="exception">The exception object</param>
        /// <returns></returns>
        protected string format(string message, Exception exception)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("{0}\r\n", message);

            builder.AppendFormat(
                "Exception Type: {0}\r\n\r\n" +
                "Exception Message: \r\n{1}\r\n\r\n" +
                "Stack Trace:\r\n{2}",
                exception.GetType().ToString(),
                exception.Message,
                exception.StackTrace);

            if (null != exception.InnerException)
            {
                builder.AppendFormat(
                "InnerException Type: {0}\r\n\r\n" +
                "InnerException Message: \r\n{1}\r\n\r\n" +
                "InnerException Stack Trace:\r\n{2}",
                exception.InnerException.GetType().ToString(),
                exception.InnerException.Message,
                exception.InnerException.StackTrace);
            }

            return builder.ToString();
        }

        #endregion

        #region Private Properties

        // This will be the mask of allowed log levels.  By default, we
        // will log everything
        private int _loggableSeverities = 0xffff;

        #endregion
    }
}
