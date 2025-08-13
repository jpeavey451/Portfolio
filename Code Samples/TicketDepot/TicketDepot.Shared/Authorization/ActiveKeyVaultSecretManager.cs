using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using System.Diagnostics.CodeAnalysis;
using Validation;

namespace TicketDepot.Shared
{
    /// <summary>
    /// KeyVaultSecretManager to load only active secret for the .
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ActiveKeyVaultSecretManager : KeyVaultSecretManager
    {
        private readonly string[] secreKeys;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveKeyVaultSecretManager"/> class.
        /// </summary>
        /// <param name="secretNames"></param>
        public ActiveKeyVaultSecretManager(string[] secretNames)
        {
            Requires.NotNull(secretNames, nameof(secretNames));
            this.secreKeys = secretNames;
        }

         /// <inheritdoc/>
        public override bool Load(SecretProperties secret) =>
            this.secreKeys.Contains(secret.Name) &&
            secret.Enabled.HasValue &&
            secret.Enabled.Value &&
            secret.ExpiresOn.HasValue &&
            secret.ExpiresOn.Value > DateTimeOffset.Now;
    }
}
