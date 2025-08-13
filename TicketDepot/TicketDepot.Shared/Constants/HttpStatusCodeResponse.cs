

namespace TicketDepot.Shared
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// This is the const class for all
    /// response strings.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpStatusCodeResponse
    {
        /// <summary>
        /// This is the response string for HttpStatusCode.Accepted.
        /// </summary>
        public const string Accepted = "Request has been accepted.";

        /// <summary>
        /// The is the response string for HttpStatusCode.AlreadyReported.
        /// </summary>
        public const string AlreadyReported = "The Request has already been submitted.";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'External Dependency Error'.
        /// </summary>
        public const string ExternalDependencyErrorCode = "External Dependency Error Code: {0}";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'internal Server Error'.
        /// </summary>
        public const string InternalServerError = "Internal Server Error";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'Invalid Request.'.
        /// </summary>
        public const string InvalidRequest = "Invalid Request.";

        /// <summary>
        /// This is the response string for
        /// data was found.
        /// </summary>
        public const string Found = "Data found.";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'Resource data not found.'.
        /// </summary>
        public const string NoDataFound = "Resource data not found.";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'MultiStatus'.
        /// </summary>
        public const string MultiStatus = "Multiple status/results.";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'NotModified'.
        /// </summary>
        public const string NotModified = "Data Not Modified.";

        /// <summary>
        /// This is the response string for
        /// requests for a pre-condition that failed.
        /// </summary>
        public const string PartialSuccess = "Partial Success.";

        /// <summary>
        /// This is the response string for
        /// requests for a pre-condition that failed.
        /// </summary>
        public const string PreconditionFailed = "Pre-condition failed";

        /// <summary>
        /// This is the response string for
        /// requests that should create a new entry.
        /// </summary>
        public const string RecordCreated = "Record Created";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'Records exist.'.
        /// </summary>
        public const string RecordExists = "Records exist";

        /// <summary>
        /// Response for success.
        /// </summary>
        public const string Success = "Operation completed successfully.";

        /// <summary>
        /// This is the response string for
        /// requests that end in 'Too Many Requests'.
        /// </summary>
        public const string TooManyRequests = "Too Many Requests";

        /// <summary>
        /// The reponse message for 'UnAuthorized'.
        /// </summary>
        public const string UnAuthorized = "User Is UnAuthorized for this operation.";

        /// <summary>
        /// Response for time outs of any nature.
        /// </summary>
        public const string TimeOut = "Operation timed out or all retries executed.";

        /// <summary>
        /// The reponse message for 'Forbidden'.
        /// </summary>
        public const string Forbidden = "Access Denied";
    }
}
