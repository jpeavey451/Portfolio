using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The repository class for Transactions.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IContainerRepository<ITransactionContainer> containerRepository;
        private readonly ILogger<TransactionRepository> logger;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IValidationProvider validationProvider;

        public TransactionRepository(
            ILogger<TransactionRepository> logger,
            IContainerRepository<ITransactionContainer> containerRepository,
            IObjectResultProvider objectResultProvider,
            IValidationProvider validationProvider)
        {
            this.containerRepository = containerRepository;
            this.logger = logger;
            this.objectResultProvider = objectResultProvider;
            this.validationProvider = validationProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> AddAsync<T>(T item, CancellationToken cancellationToken = default) where T : class
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
                this.logger.LogError(ex, "69B3A2E0-55FC-42A0-9C81-536FC921C082 Failed to get add new Transaction.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetByIdAsync<T>(string id, CancellationToken cancellationToken = default) where T : class
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
                this.logger.LogError(ex, "A9611C79-CBC8-4618-85A7-08C3D9A104CC Failed to get existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> GetByTransactionId<T>(string transactionId, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                Requires.NotNullOrWhiteSpace(transactionId, nameof(transactionId));

                QueryDefinition queryDefinition = new QueryDefinition(Queries.GetById)
                    .WithParameter(QueryParams.Id, transactionId);

                ObjectResult response = await this.containerRepository.GetItemsAsync<T>(queryDefinition).ConfigureAwait(false);
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
    }
}
