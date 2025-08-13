using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// Interface for <see cref="PaymentProvider"/>.
    /// </summary>
    public interface IPaymentProvider
    {
        /// <summary>
        /// Process payments.
        /// </summary>
        /// <param name="paymentInfo"></param>
        /// <param name="">cancellationToken</param>
        /// <returns></returns>
        Task<ObjectResult> ProcessPayment(PaymentInfo paymentInfo, CancellationToken cancellationToken = default);
    }
}
