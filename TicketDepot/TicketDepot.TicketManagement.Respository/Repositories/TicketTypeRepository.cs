using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The Repository class for <see cref="TicketType"/>.
    /// </summary>
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly ILogger<TicketTypeRepository> logger;
        private readonly IValidationProvider validationProvider;
        private IContainerRepository<ITicketTypeContainer> containerRepository;
        private IObjectResultProvider objectResultProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="TicketTypeRepository"/>.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="validationProvider"></param>
        /// <param name="containerRepository"></param>
        /// <param name="objectResultProvider"></param>
        public TicketTypeRepository(
            ILogger<TicketTypeRepository> logger,
            IValidationProvider validationProvider,
            IContainerRepository<ITicketTypeContainer> containerRepository,
            IObjectResultProvider objectResultProvider)
        {
            this.logger = logger;
            this.validationProvider = validationProvider;
            this.containerRepository = containerRepository;
            this.objectResultProvider = objectResultProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> AddAsync<T>(T item, CancellationToken cancellationToken = default)
            where T: class
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
                this.logger.LogError(ex, "B72E88A0-E8E0-428B-8F1B-47F00B7E7FC8 Failed to add new TicketType.");
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
                T? existingEvent = results.FirstOrDefault();

                if (existingEvent is null)
                {
                    return this.objectResultProvider.ResourceNotFound();
                }

                return this.objectResultProvider.Ok(existingEvent);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "1FBC373D-7431-468F-AC64-FAC8F73FBAC5 Failed to get existing Venue.");
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
