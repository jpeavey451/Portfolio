using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The <see cref="IConfidentialClientApplication"/> builder class.
    /// </summary>
    public class ClientBuilder : IClientBuilder
    {
        /// <inheritdoc/>
        public IConfidentialClientApplication CreateClientBuilder(IConfiguration configuration)
        {
            string spnName = configuration.GetSection($"{AuthConfig.AzureADSectionName}:{AuthConfig.SPNName}").Value!;
            string clientSecret = configuration.GetSection(spnName).Value!;
            string tenantId = configuration.GetSection($"{AuthConfig.AzureADSectionName}:{AuthConfig.TenantId}").Value!;
            string azureAdAuthorityBaseUrl = configuration.GetSection($"{AuthConfig.AzureADSectionName}:{AuthConfig.Instance}").Value!;
            string tenantAuthrorityUrl = $"{azureAdAuthorityBaseUrl}{tenantId}";
            string spnClientId = configuration.GetSection($"{AuthConfig.AzureADSectionName}:{AuthConfig.ClientId}").Value!;

            IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                    .Create(spnClientId) // App ID
                    .WithAuthority(tenantAuthrorityUrl)
                    .WithTenantId(tenantId)
                    .WithClientSecret(clientSecret)
                    .Build();

            return confidentialClient;
        }
    }
}
