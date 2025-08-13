using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement.Domain
{
    public class VenueProvider : IVenueProvider
    {
        private readonly ILogger<VenueProvider> logger;
        private readonly IVenueRepository venueRepository;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IVenueValidator venueValidator;

        /// <summary>
        /// Initializes a new instance of <see cref="VenueProvider"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="venueRepository"></param>
        /// <param name="venueValidator"></param>
        public VenueProvider(
            ILogger<VenueProvider> logger,
            IObjectResultProvider objectResultProvider,
            IVenueRepository venueRepository,
            IVenueValidator venueValidator)
        {
            this.logger = logger;
            this.venueRepository = venueRepository;
            this.objectResultProvider = objectResultProvider;
            this.venueValidator = venueValidator;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> CreateVenueAsync(Venue newVenue, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult validationResult = await this.venueValidator.ValidateNewVenu(newVenue, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult createResult = await this.venueRepository.AddAsync(newVenue, cancellationToken).ConfigureAwait(false);
                return createResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "065C1AFD-1B2C-4B75-BF33-33DFED4D512F Failed to create new Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

        public async Task<ObjectResult> GetAllVenues(string continuationToken, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult result = await this.venueRepository.GetAllAsync<Venue>(continuationToken, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "F268E489-AB30-49BB-8166-4DD43523350F Failed to get Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetVenueByNameAsync(string venueName, CancellationToken cancellationToken)
        {
            try
            {
                Requires.NotNullOrWhiteSpace(venueName, nameof(venueName));

                ObjectResult result = await this.venueRepository.GetByNameAsync<Venue>(venueName, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "F268E489-AB30-49BB-8166-4DD43523350F Failed to get Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetVenueByVenueIdAsync(string venueId, CancellationToken cancellationToken)
        {
            try
            {
                Requires.NotNullOrWhiteSpace(venueId, nameof(venueId));

                ObjectResult result = await this.venueRepository.GetByIdAsync<Venue>(venueId, cancellationToken).ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "E4C78B42-81E9-4BD9-B09E-1F371EB3419B Failed to get Event.");
                return this.objectResultProvider.ServerError();
            }
        }

        public async Task<ObjectResult> UpdateVenueAsync(Venue updatedVenue, CancellationToken cancellationToken)
        {
            try
            {
                ObjectResult validationResult = await this.venueValidator.ValidateUpdateVenue(updatedVenue, cancellationToken).ConfigureAwait(false);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult createResult = await this.venueRepository.UpdateAsync(updatedVenue.Id!, updatedVenue, cancellationToken).ConfigureAwait(false);
                return createResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "051C9933-1C65-4AC9-8021-34B70D2AC31D Failed to update existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
