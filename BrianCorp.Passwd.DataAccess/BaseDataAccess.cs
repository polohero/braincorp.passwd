using System;

using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Utilities;

namespace BrainCorp.Passwd.DataAccess
{
    public abstract class BaseDataAccess
    {
        public ILogger Log { get; set; }

        protected void logDebug(string message)
        {
            if (null != Log)
            {
                Log.Write(message, LogEntrySeverityEnum.Debug);
            }
        }

        protected void logDebug(string message, object obj)
        {
            if (null != Log &&
                Log.WillLog(LogEntrySeverityEnum.Debug))
            {
                Log.Write(message + ":" + ParameterChecker.IsNull(obj), LogEntrySeverityEnum.Debug);
            }
        }

        protected void logError(string message, Exception exception)
        {
            if (null != Log)
            {
                Log.Write(message, LogEntrySeverityEnum.Error, exception);
            }
        }
    }
}
