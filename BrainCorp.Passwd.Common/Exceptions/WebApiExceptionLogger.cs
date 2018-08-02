using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Common.Exceptions
{
    public class WebApiExceptionLogger : ExceptionLogger
    {
        private ILogger _logger;

        public WebApiExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            CallLogger(context);
            return base.LogAsync(context, cancellationToken);
        }

        public override void Log(ExceptionLoggerContext context)
        {
            CallLogger(context);
            base.Log(context);
        }

        private void CallLogger(ExceptionLoggerContext context)
        {
            _logger.Write(
                "There was an unhandeled exception in the WebApi application.",
                LogEntrySeverityEnum.Error,
                context.Exception);
        }
    }
}
