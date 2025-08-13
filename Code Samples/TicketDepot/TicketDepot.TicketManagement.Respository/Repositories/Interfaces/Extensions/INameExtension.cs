
using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// Get by name extension.
    /// </summary>
    public interface INameExtension
    {
        /// <summary>
        /// Gets the document by name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ObjectResult> GetByNameAsync<T>(string name, CancellationToken cancellationToken = default)
            where T: class;
    }
}
