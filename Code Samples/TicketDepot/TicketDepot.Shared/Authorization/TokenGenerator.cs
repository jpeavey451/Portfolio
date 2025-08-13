

namespace TicketDepot.Shared
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Validation;

    /// <summary>
    /// Generates Azure AD access token
    /// </summary>
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ILogger<TokenGenerator> logger;
        private readonly IConfiguration configuration;
        private readonly IClientBuilder clientBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenGenerator"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="clientBuilder"></param>
        public TokenGenerator(
            ILogger<TokenGenerator> logger,
            IConfiguration configuration,
            IClientBuilder clientBuilder)
        {
            Requires.NotNull(logger, nameof(logger));
            this.configuration = configuration;
            this.logger = logger;
            this.clientBuilder = clientBuilder;
        }

        /// <summary>
        /// The <see cref="AuthSchemes"/> scheme for this class.
        /// </summary>
        public string AuthenticationType => AuthSchemes.AzureAD;

        /// <summary>
        /// The <see cref="ConfidentialClientApplication"/> for this class.
        /// </summary>
        public IConfidentialClientApplication? ConfidentialClient { get; set; }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenAsync(string scope)
        {
            AuthenticationResult token;
            try
            {
                List<string> scopes = new List<string> { scope };
                string spnName = this.configuration.GetSection($"{AuthConfig.AzureADSectionName}:{AuthConfig.SPNName}").Value!;
                string clientSecret = this.configuration.GetSection(spnName).Value!;

                if (this.ConfidentialClient == null || !this.ConfidentialClient.AppConfig.ClientSecret.Equals(clientSecret, StringComparison.Ordinal))
                {
                    this.ConfidentialClient = this.clientBuilder.CreateClientBuilder(this.configuration);
                }

                token = await this.ConfidentialClient.AcquireTokenForClient(scopes).ExecuteAsync().ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(token?.AccessToken))
                {
                    
                    string errorMessage = $"CE7E13F7-03C9-414B-92BC-FFC857EA432D Error getting Access Token for dependent service. Token is null. Scope: {scope}";
                    this.logger.LogInformation(errorMessage);
                    throw new ExternalHttpClientException(errorMessage, HttpStatusCode.BadRequest);
                }

                return token.AccessToken;
            }
            catch (Exception ex)
            {
                string errorMessage = $"E53B0A20-39A1-478C-A883-DC04CC1422FB Error getting Access Token for dependent service. Scope: {scope} Error: {ex.Message}";
                this.logger.LogError(ex, errorMessage);
                throw new ExternalHttpClientException(errorMessage, HttpStatusCode.BadRequest);
            }
        }
    }
}