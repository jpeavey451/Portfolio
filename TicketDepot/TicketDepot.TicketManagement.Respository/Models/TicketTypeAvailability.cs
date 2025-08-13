
namespace TicketDepot.TicketManagement.Repository
{
    public class TicketTypeAvailability : TicketType
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TicketType"/>.
        /// </summary>
        /// <param name="seatingType"></param>
        /// <param name="price"></param>
        /// <param name="capacity"></param>
        /// <param name="quantity"></param>
        /// <param name="eventId"></param>
        public TicketTypeAvailability(string venueId, SeatingType seatingType, decimal price, int capacity, string eventId)
            : base(venueId, seatingType, price, capacity, eventId)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.Capacity = capacity;
            this.CreatedDate = DateTimeOffset.UtcNow;
        }

        public int TotalTicketsPurchased { get; set; }

        public int TotalTicketsReserved { get; set; }

        public int TotalAvailableTickets {  get; set; }


    }
}
