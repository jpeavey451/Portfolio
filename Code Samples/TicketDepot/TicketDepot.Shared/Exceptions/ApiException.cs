namespace TicketDepot.Shared
{
    /// <summary>
    /// The ApiException class.
    /// </summary>
    public partial class ApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        /// <param name="headers"></param>
        /// <param name="innerException"></param>
        public ApiException(string message, int statusCode, string response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            this.StatusCode = statusCode;
            this.Response = response;
            this.Headers = headers;
        }

        /// <summary>
        /// Gets or set the StatusCode.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Gets or set the Response.
        /// </summary>
        public string? Response { get; private set; }

        /// <summary>
        /// Gets or set the Headers.
        /// </summary>
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", this.Response, base.ToString());
        }
    }
}
