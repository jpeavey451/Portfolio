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
    /// The Venu Controller class.
    /// </summary>
    [ApiController]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiVersion(ApiVersioning.ApiVersionV10)]
    public class VenueController : ControllerBase
    {
        private readonly IVenueProvider venueProvider;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="VenueController"/>.
        /// </summary>
        /// <param name="venueProvider"></param>
        /// <param name="mapper"></param>
        public VenueController(
            IVenueProvider venueProvider,
            IMapper mapper)
        {
            this.venueProvider = venueProvider;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets all the Venues.
        /// </summary>
        /// <param name="continuationToken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetAllVenues(string continuationToken, CancellationToken cancellationToken)
        {
            ObjectResult result = await this.venueProvider.GetAllVenues(continuationToken, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Gets a Venue by Venue Id or returns 404 if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetVenue(string id, CancellationToken cancellationToken)
        {
            ObjectResult result = await this.venueProvider.GetVenueByVenueIdAsync(id, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Creates a new Venue. Venues are unique by name.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(VenueRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateVenueAsync([FromBody] VenueRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Venue newVenue = this.mapper.Map<Venue>(request);
            ObjectResult result = await this.venueProvider.CreateVenueAsync(newVenue, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Gets a Venue by name or returns 404 if not found.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/name/{name}")]
        [ProducesResponseType(typeof(Venue), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
        {
            ObjectResult result = await this.venueProvider.GetVenueByNameAsync(name, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }


        /// <summary>
        /// Updates an existing Venue. Venues are unique by name.
        /// </summary>
        /// <param name="updatedVenue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(VenueRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut]
        public async Task<IActionResult> UpdateVenue([FromBody] Venue updatedVenue, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ObjectResult result = await this.venueProvider.UpdateVenueAsync(updatedVenue, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }
    }
}
