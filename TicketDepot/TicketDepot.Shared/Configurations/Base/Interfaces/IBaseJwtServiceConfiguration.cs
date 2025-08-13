

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base dependent JWT service configuration interface.
    /// </summary>
    public interface IBaseJwtServiceConfiguration : IBaseServiceConfiguration
    {
        /// <summary>
        /// Gets or sets the Authorization SPNClientId.
        /// Note: This may not be the Client ID of the service being called.
        /// </summary>
        public string SPNClientId { get; set; }

        /// <summary>
        /// Gets the Scope.
        /// </summary>
        public string Scope { get; }
    }
}
