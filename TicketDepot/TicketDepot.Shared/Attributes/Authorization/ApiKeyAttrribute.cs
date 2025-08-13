using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.Shared
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute()
            : base(typeof(ApiKeyAuthFilter))
        {
        }
    }
}
