using System;
using System.Collections.Concurrent;

using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.WebService.Diagnostics
{
    public class DiagnosticsLogger : Logger
    {
        private static ConcurrentQueue<string> _logs =
             new ConcurrentQueue<string>();

        private static long _numberOfCalls = 0;

        private static double _avgDuration = 0;

        private const int NUMBER_OF_LOGS_TO_KEEP = 25;

        public static long NumberOfCalls { get { return _numberOfCalls; } }
        public static double Avg { get { return _avgDuration; } }

        public static string[] Logs { get { return _logs.ToArray(); } }

        public static void LogCall(long duration)
        {            
            _avgDuration = (_avgDuration * _numberOfCalls + duration) / (_numberOfCalls + 1);
            _numberOfCalls++;
        }

        public override void SetLoggableSeverities(LogEntrySeverityEnum severityMask)
        {
            
        }

        public override bool WillLog(LogEntrySeverityEnum severity)
        {
            return true;
        }

        public override void Write(string message, LogEntrySeverityEnum severity)
        {
            try
            {
                if(_logs.Count > NUMBER_OF_LOGS_TO_KEEP)
                {
                    _logs.TryDequeue(out string throwAway);
                }

                _logs.Enqueue(message);
            }
            catch(Exception)
            {
                //Swallow
            }
        }

        public override void Write(string message, LogEntrySeverityEnum severity, Exception exception)
        {
            try
            {
                if (_logs.Count > NUMBER_OF_LOGS_TO_KEEP)
                {
                    _logs.TryDequeue(out string throwAway);
                }

                _logs.Enqueue(format(message, exception));
            }
            catch (Exception)
            {
                //Swallow
            }
        }
    }


}