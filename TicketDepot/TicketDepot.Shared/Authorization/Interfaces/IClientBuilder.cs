using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The interface for <see cref="ClientBuilder"/>.
    /// </summary>
    public interface IClientBuilder
    {
        /// <summary>
        /// Creates the <see cref="ConfidentialClientApplication"/>.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>The instance of <see cref="IConfidentialClientApplication"/>.</returns>
        IConfidentialClientApplication CreateClientBuilder(IConfiguration configuration);
    }
}