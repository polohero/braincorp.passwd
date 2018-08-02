
namespace BrainCorp.Passwd.Common.Logging
{
    /// <summary>
    /// Define our own log severity.  This
    /// will allow us to extend/change to other
    /// loggers without having a hard dependency
    /// on what the Win32 event log defines.
    /// </summary>
    public enum LogEntrySeverityEnum
    {
        Debug = 0x02,
        Success = 0x04,
        Failure = 0x08,
        Error = 0x10,
        Warning = 0x20,
    }
}
