
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The TokenStore class.
    /// </summary>
    public class TokenStore : ITokenStore
    {
        private const int TokenExpirationBufferTime = 15;
        private readonly ConcurrentDictionary<ServiceType, ApiToken> tokenCache = new ConcurrentDictionary<ServiceType, ApiToken>();
        private readonly IConfiguration configuration;

        private readonly TokenGeneratorFactory tokenGeneratorFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenStore"/>.
        /// </summary>
        /// <param name="tokenGeneratorFactory"></param>
        /// <param name="configuration"></param>
        public TokenStore(TokenGeneratorFactory tokenGeneratorFactory, IConfiguration configuration)
        {
            this.tokenGeneratorFactory = tokenGeneratorFactory;
            this.configuration = configuration;
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenAsync(ServiceType scopeType, string scope)
        {
            if (this.tokenCache.TryGetValue(scopeType, out ApiToken? token) &&
                token?.ValidTo > DateTime.UtcNow.AddMinutes(TokenExpirationBufferTime) &&
                    token.AppRegistrationScope.Equals(scope, StringComparison.OrdinalIgnoreCase))
            {
                if (token != null)
                {
                    return token.TokenValue;
                }
            }

            this.tokenCache.TryRemove(scopeType, out _);

            string tokenValue;

            string auth0EnabledServices = this.configuration.GetValue(AuthConfig.Auth0Enabled, string.Empty);
            List<string>? auth0Enabled = !string.IsNullOrWhiteSpace(auth0EnabledServices) ?
                                        auth0EnabledServices.ToLower(CultureInfo.InvariantCulture).DeserializeObject<List<string>>()
                                        : null;

            if (auth0Enabled != null && auth0Enabled.Contains(scopeType.ToString().ToLower(CultureInfo.InvariantCulture)))
            {
                tokenValue = await this.tokenGeneratorFactory.GetTokenGenerator(AuthSchemes.Auth0).GetAccessTokenAsync(scope).ConfigureAwait(false);
            }
            else
            {
                tokenValue = await this.tokenGeneratorFactory.GetTokenGenerator(AuthSchemes.AzureAD).GetAccessTokenAsync(scope).ConfigureAwait(false);
            }

            JwtSecurityToken jwt = new JwtSecurityToken(tokenValue);
            ApiToken apiToken = new ApiToken(scope, tokenValue, jwt.ValidTo);
            this.tokenCache.GetOrAdd(scopeType, apiToken);

            return apiToken.TokenValue;
        }
    }
}