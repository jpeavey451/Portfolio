
namespace TicketDepot.Shared
{
    /// <summary>
    /// Authentication and Authorization related configuration.
    /// </summary>
    public interface IServiceAuthorizationConfiguration
    {
        /// <summary>
        /// Section Name as defined in Settings.xml
        /// </summary>
        public const string SectionName = AuthConfig.AzureADSectionName;

        /// <summary>
        /// Tenant ID.
        /// </summary>
        public string? TenantId { get; set; }

        /// <summary>
        /// Service Principal Client ID used by User Service as Identity in Active Directory. Different per environment/tenant.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Name of the secret in key vault.
        /// </summary>
        public string? SPNName { get; set; }

        /// <summary>
        /// Key Vault URL where secret associated with SPNs exist. For example: https://tiketdepot.vault.azure.net/
        /// </summary>
        public string? KeyVaultURL { get; set; }

        /// <summary>
        /// Reload Interval for KeyVault secrets, as a <see cref="TimeSpan"/>.
        /// </summary>
        public string? ReloadInterval { get; set; }

        /// <summary>
        /// Azure AD endpoint to retrieve JWT token as well as used in Authority Validation of Bearer token.
        /// </summary>
        public string? Instance { get; set; }

        /// <summary>
        /// Azure AD AuthorizationUrl
        /// </summary>
        public Uri AuthorizationUrl
        {
            get { return new Uri($"{this.Instance}{this.TenantId}/oauth2/v2.0/authorize");  }
        }

        /// <summary>
        /// Azure AD Scope
        /// </summary>
        public string? Scope
        {
            get { return $"api://{this.ClientId}/.default"; }
        }
    }
}
