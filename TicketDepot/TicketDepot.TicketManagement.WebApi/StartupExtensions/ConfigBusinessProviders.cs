using TicketDepot.TicketManagement.Domain;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// Adds the business providers to DI.
    /// </summary>
    public static class ConfigBusinessProviders
    {
        /// <summary>
        /// Adds the business providers to DI.
        /// </summary>
        /// <param name="services"></param>
        public static void AddBusinessProviders(this IServiceCollection services)
        {
            services.AddScoped<IEventProvider, EventProvider>();
            services.AddScoped<IReservationProvider, ReservationProvider>();
            services.AddScoped<IVenueProvider, VenueProvider>();
            services.AddScoped<ITicketTypeProvider, TicketTypeProvider>();
        }
    }
}
