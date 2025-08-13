

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base dependent service configuration class.
    /// </summary>
    public abstract class BaseDependentServiceConfiguration : IBaseServiceConfiguration
    {
        /// <inheritdoc/>
        public abstract string SectionName { get; }

        /// <inheritdoc/>
        public string ApiVersion { get; set; }

        /// <inheritdoc/>
        public string BaseAddress { get; set; }
    }
}
