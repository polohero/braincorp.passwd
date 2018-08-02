using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace BrainCorp.Passwd.Common.Exceptions
{
    public class WebApiUnhandledExceptionResult : IHttpActionResult
    {
        private ExceptionHandlerContext _context;

        public WebApiUnhandledExceptionResult(ExceptionHandlerContext context)
        {
            _context = context;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            string message = string.Format("There was an unhandled exception in {0}.", _context.Request.RequestUri);
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                ReasonPhrase = message,
                Content = new StringContent(string.Format("Message: {0}\nException: {1}", message, _context.Exception))
            };

            return Task.FromResult(httpResponseMessage);
        }
    }
}
