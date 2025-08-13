
using System.ComponentModel.DataAnnotations;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The VenueRequest DTO class.
    /// </summary>
    public class VenueRequest : BaseVenue
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Venue"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="capacity"></param>
        public VenueRequest(string name, string description)
            : base(name, description)
        {
        }
    }
}
