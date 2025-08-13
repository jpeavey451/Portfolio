

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base dependent ApiKey service configuration interface.
    /// </summary>
    public interface IBaseApiKeyServiceConfiguration : IBaseServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the ApiKeyPrimary.
        /// </summary>
        public string ApiKeyPrimary { get; set; }

        /// <summary>
        /// Gets or sets the ApiKeySecondary
        /// </summary>
        public string ApiKeySecondary { get; set; }
    }
}
