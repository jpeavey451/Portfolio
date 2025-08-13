
using System.Diagnostics.CodeAnalysis;
using TicketDepot.Shared;

namespace TicketDepot.Shared
{
    /// <summary>
    /// Authentication and Authorization related configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ServiceAuthorizationConfiguration : IServiceAuthorizationConfiguration
    {
        /// <summary>
        /// Section Name as defined in Settings.xml
        /// </summary>
        public const string SectionName = "AzureAd";

        /// <inheritdoc/>
        public string? TenantId { get; set; }

        /// <inheritdoc/>
        public string? ClientId { get; set; }

        /// <inheritdoc/>
        public string? SPNName { get; set; }

        /// <inheritdoc/>
        public string? KeyVaultURL { get; set; }

        /// <inheritdoc/>
        public string? ReloadInterval { get; set; }

        /// <inheritdoc/>
        public string? Instance { get; set; }
    }
}