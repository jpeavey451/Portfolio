using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The Transaction extensions interface.
    /// </summary>
    public interface ITransactionExtension
    {
        /// <summary>
        /// Gets the items by the specified transaction id.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetByTransactionId<T>(string transactionId, CancellationToken cancellationToken = default)
            where T : class;
    }
}
