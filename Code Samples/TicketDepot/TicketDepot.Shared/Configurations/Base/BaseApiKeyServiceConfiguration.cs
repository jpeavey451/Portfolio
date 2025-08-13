

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base ApiKey service configuration class.
    /// </summary>
    public abstract class BaseApiKeyServiceConfiguration : BaseDependentServiceConfiguration, IBaseApiKeyServiceConfiguration
    {
        /// <inheritdoc/>
        public string ApiKeyPrimary { get; set; }

        /// <inheritdoc/>
        public string ApiKeySecondary { get; set; }
    }
}
