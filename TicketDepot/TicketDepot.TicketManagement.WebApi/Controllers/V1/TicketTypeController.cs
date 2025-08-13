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
    /// The TicketType controller class.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    [ApiVersion(ApiVersioning.ApiVersionV10)]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketTypeProvider ticketTypeProvider;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of <see cref="TicketTypeController"/>.
        /// </summary>
        /// <param name="ticketTypeProvider"></param>
        /// <param name="mapper"></param>
        public TicketTypeController(
            ITicketTypeProvider ticketTypeProvider,
            IMapper mapper)
        {
            this.ticketTypeProvider = ticketTypeProvider;
            this.mapper = mapper;
        }

        /// <summary>
        /// Creates a new TicketType for the specified EventId.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TicketType), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> CreateTicketTypeAsync([FromBody] TicketTypeRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TicketType newTicketType = this.mapper.Map<TicketType>(request);
            ObjectResult result = await this.ticketTypeProvider.CreateTicketTypeAsync(newTicketType, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }

        /// <summary>
        /// Updates a new TicketType for the specified EventId.
        /// </summary>
        /// <param name="updatedTicketType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(TicketType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<IActionResult> UpdateTicketTypeAsync([FromBody] TicketType updatedTicketType, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ObjectResult result = await this.ticketTypeProvider.UpdateTicketTypeAsync(updatedTicketType, cancellationToken).ConfigureAwait(false);
            if (result.StatusCode != (int)HttpStatusCode.OK)
            {
                return result;
            }

            return Ok(result.Value);
        }
    }
}
