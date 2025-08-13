using TicketDepot.TicketManagement.Domain;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// AutoMapper configuration class.
    /// </summary>
    public class ModelMapper : AutoMapper.Profile
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModelMapper"/>.
        /// </summary>
        public ModelMapper()
        {
            CreateMap<EventRequest, Event>();
            CreateMap<VenueRequest, Venue>();
        }
    }
}
