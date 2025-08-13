using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The TicketType Request class.
    /// </summary>
    public class TicketTypeRequest : BaseTicketType
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TicketTypeRequest"/>.
        /// </summary>
        /// <param name="seatingType"></param>
        /// <param name="price"></param>
        /// <param name="quantity"></param>
        /// <param name="eventId"></param>
        public TicketTypeRequest(string venueId, SeatingType seatingType, decimal price, string eventId)
            : base(venueId, seatingType, price, eventId)
        {

        }
    }
}
