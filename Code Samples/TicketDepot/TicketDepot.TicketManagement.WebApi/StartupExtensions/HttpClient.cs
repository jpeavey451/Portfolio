using Microsoft.Extensions.Options;
using TicketDepo.TicketManagement.Clients.Interfaces;
using TicketDepot.Shared;

namespace TicketDepot.TicketManagement.WebApi.StartupExtensions
{
    /// <summary>
    /// Adds HttpClients to the DI
    /// </summary>
    public static class HttpClient
    {
        /// <summary>
        /// Calls the method to create teh <see cref="HttpClient"/>
        /// for each dependent service.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        public static void SetupHttpClients(this IServiceCollection services, IConfiguration config)
        {
            // Uses JWT
            services.SetupPaymentClient();
        }

        /// <summary>
        /// Setups the Notification Client using JWT.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        private static void SetupPaymentClient(this IServiceCollection services)
        {
            services
                .AddHttpClient<IPaymentServiceClient, IPaymentServiceClient>((provider, client) =>
                {
                    PaymentServiceConfiguration paymentServiceConfig = provider.GetRequiredService<IOptions<PaymentServiceConfiguration>>().Value;
                    client.BaseAddress = new Uri(paymentServiceConfig.BaseAddress);
                })
                .AddHttpMessageHandler(provider =>
                {
                    PaymentServiceConfiguration paymentServiceConfig = provider.GetRequiredService<IOptions<PaymentServiceConfiguration>>().Value;
                    BearerTokenHandler handler = provider.GetRequiredService<BearerTokenHandler>();
                    handler.ServiceType = ServiceType.PaymentService;
                    handler.ApplicationRegistrationScope = $"api://{paymentServiceConfig.SPNClientId}/.default";
                    return handler;
                });
        }
    }
}
