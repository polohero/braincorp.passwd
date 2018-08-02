using System;

namespace BrainCorp.Passwd.Common.Exceptions
{
    /// <summary>
    /// A Hard Failure exception is used to distinguish from a known
    /// exception that will cause the operation to never succeed vs an unknown exception.
    /// Examples where this will be used, say a service receives a request
    /// and the request is missing a key field that causes it to not handle a request.
    /// The hard failure exception will be thrown instead of an ArgumentNull. The goal
    /// of a hard failure is that the service can handle the response to the caller 
    /// differently if it's a known hard failure instead of an unknown hard failure.
    /// </summary>
    public class HardFailureException : Exception
    {
        public HardFailureException() { }

        public HardFailureException(string message) : base(message) { }

        public HardFailureException(string message, Exception exception) : base(message, exception) { }
    }
}
