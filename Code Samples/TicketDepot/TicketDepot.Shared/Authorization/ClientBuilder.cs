using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The <see cref="IConfidentialClientApplication"/> builder class.
    /// </summary>
    public class ClientBuilder : IClientBuilder
    {
        private readonly ServiceAuthorizationConfiguration serviceConfig;

        /// <summary>
        /// Initializes a new instance of <see cref="ClientBuilder"/>
        /// </summary>
        /// <param name="serviceConfig"></param>
        public ClientBuilder(IOptions<ServiceAuthorizationConfiguration> serviceConfig)
        {
            this.serviceConfig = serviceConfig.Value;
        }

        /// <inheritdoc/>
        public IConfidentialClientApplication CreateClientBuilder(IConfiguration configuration)
        {
            string clientSecret = configuration.GetSection(this.serviceConfig.SPNName!).Value!;

            IConfidentialClientApplication confidentialClient = ConfidentialClientApplicationBuilder
                    .Create(this.serviceConfig.ClientId! ) // App ID
                    .WithAuthority( $"{this.serviceConfig.Instance!}{this.serviceConfig.TenantId!}" )
                    .WithTenantId( this.serviceConfig.TenantId! )
                    .WithClientSecret(clientSecret)
                    .Build();

            return confidentialClient;
        }
    }
}
