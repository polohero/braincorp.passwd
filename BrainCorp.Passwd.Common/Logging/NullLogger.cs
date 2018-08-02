using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainCorp.Passwd.Common.Logging
{
    public class NullLogger : ILogger
    {
        public void SetLoggableSeverities(LogEntrySeverityEnum severityMask)
        {
           
        }

        public bool WillLog(LogEntrySeverityEnum severity)
        {
            return false;
        }

        public void Write(string message, LogEntrySeverityEnum severity)
        {
           
        }

        public void Write(string message, LogEntrySeverityEnum severity, Exception exception)
        {
       
        }
    }
}
