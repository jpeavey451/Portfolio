

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base JWT service configuration class.
    /// </summary>
    public abstract class BaseJwtServiceConfiguration : BaseDependentServiceConfiguration, IBaseJwtServiceConfiguration
    {
        /// <inheritdoc/>
        public string SPNClientId { get; set; }

        /// <inheritdoc/>
        public abstract string Scope { get; }
    }
}
