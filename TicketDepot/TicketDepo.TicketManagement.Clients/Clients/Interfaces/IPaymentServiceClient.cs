using Microsoft.AspNetCore.Mvc;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepo.TicketManagement.Clients.Interfaces
{
    /// <summary>
    /// The interface for <see cref="PaymentServiceClient"/>
    /// </summary>
    public partial interface IPaymentServiceClient
    {
        /// <summary>Create a module</summary>
        /// <param name="api_version">The requested API version</param>
        /// <param name="api_version">The requested API version</param>
        /// <param name="body">The payment processsing request details.</param>
        /// <returns>The <see cref="ObjectResult"/> indicating success/failure.</returns>
        Task<ObjectResult> ProcessPayment(string api_version, string ifMatch, PaymentInfo body, CancellationToken cancellationToken = default);
    }
}
