using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;


namespace BrainCorp.Passwd.Common.Exceptions
{
    public class WebApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HandleUnhandledException(context);
        }

        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            HandleUnhandledException(context);
            return Task.FromResult(0);
        }

        private void HandleUnhandledException(ExceptionHandlerContext context)
        {
            context.Result = new WebApiUnhandledExceptionResult(context);
        }
    }
}
