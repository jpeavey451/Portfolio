using System.Net;

namespace TicketDepot.Shared
{
    /// <summary>
    /// Class for external http client exceptions.
    /// </summary>
    public class ExternalHttpClientException : Exception
    {
        public ExternalHttpClientException(string message, HttpStatusCode statusCode)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
