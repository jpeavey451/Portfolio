using System.ComponentModel.DataAnnotations;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The EventRequest DTO class.
    /// </summary>
    public class EventRequest : BaseEvent
    {
        /// <summary>
        /// Instantiates a new instance of <see cref="EventRequest"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="venueId"></param>
        /// <param name="description"></param>
        /// <param name="eventType"></param>
        /// <param name="eventDate"></param>
        /// <param name="eventStartTime"></param>
        public EventRequest(string name, string venueId, string description, EventType eventType, DateOnly eventDate, TimeOnly eventStartTime)
            : base(name, venueId, description, eventType, eventDate, eventStartTime)
        {
        }
    }
}
