using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Extension interface for GetAll.
    /// </summary>
    public interface IGetAllExtension
    {
        /// <summary>
        /// Gets the entire collection, paginated.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="continuationToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetAllAsync<T>(string continuationToken, CancellationToken cancellationToken = default)
            where T : class;
    }
}
