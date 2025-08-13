
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    /// <summary>
    /// The TicketType Provider class.
    /// </summary>
    public class TicketTypeProvider : ITicketTypeProvider
    {
        private readonly ILogger<TicketTypeProvider> logger;
        private readonly ITicketTypeRepository ticketTypeRepository;
        private readonly IEventRepository eventRepository;
        private readonly IVenueRepository venueRepository;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;
        private readonly ITicketTypeValidator ticketTypeValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="TicketTypeProvider"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="ticketTypeRepository"></param>
        /// <param name="eventRepository"></param
        /// <param name="venueRepository"></param>
        /// <param name="validationProvider"></param>
        public TicketTypeProvider(
            ILogger<TicketTypeProvider> logger,
            IObjectResultProvider objectResultProvider,
            ITicketTypeRepository ticketTypeRepository,
            IEventRepository eventRepository,
            IVenueRepository venueRepository,
            ITicketTypeValidator ticketTypeValidator,
            IValidationProvider validationProvider)
        {
            this.logger = logger;
            this.ticketTypeRepository = ticketTypeRepository;
            this.eventRepository = eventRepository;
            this.venueRepository = venueRepository;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
            this.ticketTypeValidator = ticketTypeValidator;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> CreateTicketTypeAsync(TicketType newTicketType, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult validationResult = await this.ticketTypeValidator.ValidateNewTicketType(newTicketType, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }
                
                ObjectResult createResult = await this.ticketTypeRepository.AddAsync(newTicketType, cancellationToken).ConfigureAwait(false);
                return createResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "065C1AFD-1B2C-4B75-BF33-33DFED4D512F Failed to create new TicketType.");
                return this.objectResultProvider.ServerError();
            }
        }

        public async Task<ObjectResult> UpdateTicketTypeAsync(TicketType updatedTicketType, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult validationResult = await this.ticketTypeValidator.ValidateUpdatedTicketType(updatedTicketType).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult createResult = await this.ticketTypeRepository.AddAsync(updatedTicketType, cancellationToken).ConfigureAwait(false);
                return createResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "FF9ED70E-1F73-4D68-8F2B-66BEF73F0D32 Failed to update existing TicketType.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
