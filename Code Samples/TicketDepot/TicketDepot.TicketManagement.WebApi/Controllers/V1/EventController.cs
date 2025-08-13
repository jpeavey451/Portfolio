using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Domain;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.WebApi.Controllers.V1
{
    /// <summary>
    /// Then Events Controller class.
    /// </summary>
    [Authorize]
    [ApiVersion(ApiVersioning.ApiVersionV10)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventProvider eventProvider;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance <see cref="EventsController"/>.
        /// </summary>
        /// <param name="eventProvider"></param>
        /// <param name="mapper"></param>
        public EventsController(
            IEventProvider eventProvider,
            IMapper mapper)
        {
            this.eventProvider = eventProvider;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new Event. Events are unique by name.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event newEvent = this.mapper.Map<Event>(request);
            ObjectResult result = await this.eventProvider.CreateEventAsync(newEvent, cancellationToken).ConfigureAwait(false);
            return Ok(result.Value);
        }


        /// <summary>
        /// Creates a new Event. Events are unique by name.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateEventAsync([FromBody] Event request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            ObjectResult result = await this.eventProvider.UpdateEventAsync(request, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Reserves anset of tickets for a <see cref="TicketType"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("reserve")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> ReserveTicketsAsync([FromBody] ReservationRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Reservation reservation = this.mapper.Map<Reservation>(request);
            ObjectResult result = await this.eventProvider.ReserveTicketsAsync(reservation, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Cancels a set of tickets for a <see cref="Reservation"/>.
        /// </summary>
        /// <param name="reservationId"></param>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("cancel")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CancelTicketsAsync(string reservationId, string transactionId, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ObjectResult result = await this.eventProvider.CancelTicketsAsync(reservationId, transactionId, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }


        /// <summary>
        /// Cancells a set of tickets for a <see cref="Reservation"/>.
        /// </summary>
        /// <param name="reservation"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("purchase")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> PurchaseTicketsAsync([FromBody] Reservation reservation, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ObjectResult result = await this.eventProvider.PurchaseTicketsAsync(reservation, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }
        /// <summary>
        /// Gets the List of TicketType availability by <see cref="SeatingType"/>.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{eventId}/availability")]
        [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAvailabilityAsync(string eventId, CancellationToken cancellationToken)
        {
            ObjectResult result = await this.eventProvider.GetAvailabilityAsync(eventId, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }
    }

}
