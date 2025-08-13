
namespace TicketDepot.Shared
{
    /// <summary>
    /// Constants class for auth settings.
    /// </summary>
    public static class AuthConfig
    {

        public const string ApiKeyHeaderName = "X-API-Key";

        public const string ApiKeyName = "ApiKey";

        public const string PaymentServiceSectionName = "PaymentService";

        public const string AzureADSectionName = "AzureAd";

        public const string SPNName = "SPNName";
        public const string TenantId = "TenantId";
        public const string Instance = "Instance";
        public const string ClientId = "ClientId";
        public const string Auth0Enabled = "Auth0Enabled";
    }
}
