using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.Common
{
    public class TestContextLogger : Logger
    {
        public TestContext Context { get; set; }

        public TestContextLogger(TestContext context)
        {
            Context = context;
        }

        /// <summary>
        /// This method will write the message to the 
        /// log along with the corresponding severity
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="severity">The severity of the log entry</param>
        public override void Write(string message, LogEntrySeverityEnum severity)
        {
            try
            {
                write(message, severity);
            }
            catch (Exception)
            {
                // Log failures should not 
                // cause our program to crash
            }
        }

        /// <summary>
        /// This method will write the message to the log
        /// along with select attributes from the exception object
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        /// <param name="severity">The severity of the event</param>
        /// <param name="exception">The exception object</param>
        public override void Write(string message, LogEntrySeverityEnum severity, Exception exception)
        {
            try
            {
                write(format(message, exception), severity);
            }
            catch (Exception)
            {
                // Log failures should not 
                // cause our program to crash
            }
        }

        /// <summary>
        /// Writes the actual data to the log.
        /// </summary>
        /// <param name="message">The text to log</param>
        /// <param name="severity">The severity of the log entry</param>
        private void write(string message, LogEntrySeverityEnum severity)
        {
            Context.WriteLine("[" + DateTime.Now + "]" + message);
        }


    }
}
