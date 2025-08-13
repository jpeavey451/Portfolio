using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    public interface IUpdateExtension
    {
        Task<ObjectResult> UpdateAsync<T>(string id, T item, CancellationToken cancellationToken = default)
            where T : class;
    }
}
