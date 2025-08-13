
using Microsoft.AspNetCore.Mvc;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The Payment Processor class.
    /// </summary>
    public class PaymentProvider : IPaymentProvider
    {
        private readonly IObjectResultProvider objectResultProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="PaymentProvider"/>.
        /// </summary>
        /// <param name="objectResultProvider"></param>
        public PaymentProvider(
            IObjectResultProvider objectResultProvider)
        {
            this.objectResultProvider = objectResultProvider;
        }

        /// <inheritdoc/>
        public Task<ObjectResult> ProcessPayment(PaymentInfo paymentInfo, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.objectResultProvider.Ok());
        }
    }
}
