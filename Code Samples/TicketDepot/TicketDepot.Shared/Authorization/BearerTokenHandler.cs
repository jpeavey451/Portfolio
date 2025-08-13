
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using Validation;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The Bearer Token Handler class.
    /// </summary>
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly ILogger<BearerTokenHandler> logger;
        private readonly ITokenStore tokenStore;

        /// <summary>
        /// Initializes a new instance of <see cref="BearerTokenHandler"/>.
        /// </summary>
        /// <param name="tokenStore"></param>
        /// <param name="logger"></param>
        public BearerTokenHandler(ITokenStore tokenStore, ILogger<BearerTokenHandler> logger)
        {
            Requires.NotNull(tokenStore, nameof(tokenStore));
            Requires.NotNull(logger, nameof(logger));

            this.tokenStore = tokenStore;
            this.logger = logger;
        }

        public ServiceType ServiceType { get; set; }

        public string? ApplicationRegistrationScope { get; set; }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                Requires.NotNull( request, nameof( request ) );
                Requires.NotNullOrWhiteSpace( this.ApplicationRegistrationScope!, nameof( this.ApplicationRegistrationScope ));

                // request the access token
                string accessToken = await this.tokenStore.GetAccessTokenAsync(this.ServiceType, this.ApplicationRegistrationScope).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(accessToken))
                {
                    string errorMessage = $"F5F74CFA-F7B7-4E97-8E9A-34BCC610436F Error getting Token for dependent service. Token is null. ServiceType: {this.ServiceType} AppReg Scope: {this.ApplicationRegistrationScope}";
                    this.logger.LogInformation(errorMessage);
                    throw new ExternalHttpClientException(errorMessage, HttpStatusCode.BadRequest);
                }

                // set the bearer token to the outgoing request
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                return response;
            }
            catch ( ExternalHttpClientException ex )
            {
                this.logger.LogError(ex, $"1FB46F21-FAD8-4B25-83B2-877F9E4895C1, Error sending GetAccessToken request. ServiceType: {this.ServiceType} AppReg Scope: {this.ApplicationRegistrationScope} Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"538474CC-A5F4-465D-AE24-F7AC621BF108 Error sending GetAccessToken request. ServiceType: {this.ServiceType} AppReg Scope: {this.ApplicationRegistrationScope} Error: {ex.Message}";
                this.logger.LogError(ex, errorMessage);
                throw new ExternalHttpClientException(errorMessage, HttpStatusCode.BadRequest );
            }
        }
    }
}