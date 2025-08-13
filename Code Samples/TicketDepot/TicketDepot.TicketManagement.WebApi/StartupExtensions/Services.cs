using TicketDepot.Shared;
using TicketDepot.TicketManagement.Domain;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// Add Misc Services to DI.
    /// </summary>
    internal static class Services
    {
        /// <summary>
        /// Adds Misc services to DI.
        /// </summary>
        /// <param name="services"></param>
        internal static void AddMiscServices(this IServiceCollection services)
        {
            services.AddScoped<IObjectResultProvider, ObjectResultProvider>();
            services.AddScoped<IValidationProvider, ValidationProvider>();
            services.AddScoped<ITicketTypeValidator, TicketTypeValidator>();
            services.AddScoped<IPaymentProvider, PaymentProvider>();
            services.AddScoped<IEventValidator, EventValidator>();
            services.AddScoped<IVenueValidator, VenueValidator>();

            services.AddTransient<IApiKeyValidation, ApiKeyValidation>();
            services.AddScoped<ApiKeyAuthFilter>();
        }
    }
}
