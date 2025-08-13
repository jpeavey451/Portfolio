
namespace TicketDepot.Shared
{
    public class CosmosQueryResults<T>
    {
        public List<T>? Results { get; set; }

        public string? ResponseContinuation { get; set; }
    }
}
