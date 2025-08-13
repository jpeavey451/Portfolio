using Microsoft.Extensions.Configuration;

namespace TicketDepot.Shared
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        public readonly IConfiguration configuration;

        public ApiKeyValidation(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool IsValidApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return false;
            }

            string apiConfigKey = this.configuration.GetValue<string>(AuthConfig.ApiKeyName) ?? string.Empty;
            if (!apiKey.Equals(apiConfigKey, StringComparison.Ordinal))
            { 
                return false;
            }

            return true;
        }
    }
}
