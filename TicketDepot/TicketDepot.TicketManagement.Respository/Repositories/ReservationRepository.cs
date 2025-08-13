using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepot.TicketManagement
{
    /// <summary>
    /// The repository class for <see cref="Reservation"/>.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly IContainerRepository<IReservationContainer> containerRepository;
        private readonly ILogger<VenueRepository> logger;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="ReservationRepository"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="containerRepository"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="validationProvider"></param>
        public ReservationRepository(
            ILogger<VenueRepository> logger,
            IContainerRepository<IReservationContainer> containerRepository,
            IObjectResultProvider objectResultProvider,
            IValidationProvider validationProvider)
        {
            this.containerRepository = containerRepository;
            this.logger = logger;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> AddAsync<T>(T item, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNull(item, nameof(item));

                ObjectResult validationResult = this.validationProvider.ValidateObject(item);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult addResult = await this.containerRepository.AddItemAsync(item).ConfigureAwait(false);
                return addResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "30955222-D8BF-4FF4-935B-D6D21A137841 Failed to add new Reservation.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetAllByEventIdAsync<T>(string eventId, string? continuationToken = null, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                Requires.NotNullOrWhiteSpace(eventId, nameof(eventId));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetAllByEventId)
                    .WithParameter(QueryParams.EventId, eventId);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition, continuationToken, cancellationToken);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                return this.objectResultProvider.Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "C8A50391-D970-492E-BF3C-68DB66235766 Failed to get all Venues.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetByIdAsync<T>(string id, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNullOrWhiteSpace(id, nameof(id));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetById)
                    .WithParameter(QueryParams.Id, id);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                List<T>? results = response.Value! as List<T> ?? new List<T>();
                T? existingReservation = results.FirstOrDefault();

                if (existingReservation is null)
                {
                    return this.objectResultProvider.ResourceNotFound();
                }

                return this.objectResultProvider.Ok(existingReservation);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "9502AC9C-51B0-4928-BD11-43F9DE168BE9 Failed to get existing Reservation.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetCountOfTicketsReservedAsync(string eventId, CancellationToken cancellationToken = default)
        {
            try
            {
                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetCountOfTicketsReserved)
                    .WithParameter(QueryParams.EventId, eventId);

                ObjectResult response = await this.containerRepository.GetItemsAsync<Reservation>(queryDefinition, null, cancellationToken);
                if (response.StatusCode != (int)HttpStatusCode.OK)
                {
                    return response;
                }

                return this.objectResultProvider.Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "D6D54EB1-9740-4B64-91D2-E9C155B083F9 Failed to get all Venues.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> UpdateAsync<T>(string id, T item, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                Requires.NotNull(item, nameof(item));

                ObjectResult validationResult = this.validationProvider.ValidateObject(item);
                if (validationResult.StatusCode != (int)HttpStatusCode.OK)
                {
                    return validationResult;
                }

                ObjectResult addResult = await this.containerRepository.UpdateItemAsync(id, item).ConfigureAwait(false);
                return addResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "FCA4B92A-A35B-42FD-B28D-6E10C6E32FF7 Failed to update Event.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
