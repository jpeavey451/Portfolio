using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.TicketManagement.Repository
{
    public interface IDeleteExtension
    {
        Task<ObjectResult> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}
