using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainCorp.Passwd.Common.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Attempts to write a logging message if the severity is such that
        /// it should be logged.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        void Write(string message, LogEntrySeverityEnum severity);

        /// <summary>
        /// Attempts to write a logging message if the severity is such that
        /// it should be logged.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        void Write(string message, LogEntrySeverityEnum severity, Exception exception);

        bool WillLog(LogEntrySeverityEnum severity);

        /// <summary>
        /// Assigns a logging severity mask so determine if the logging should be logged.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severity"></param>
        void SetLoggableSeverities(LogEntrySeverityEnum severityMask);
    }
}
