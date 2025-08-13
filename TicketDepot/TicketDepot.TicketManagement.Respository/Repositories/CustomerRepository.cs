using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System.Net;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The repository for <see cref="Customer"/>.
    /// </summary>
    public class CustomerRepository : ICustomerRespository
    {
        private readonly ILogger<CustomerRepository> logger;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IContainerRepository<ICustomerContainer> containerRepository;

        public CustomerRepository(
            ILogger<CustomerRepository> logger,
            IContainerRepository<ICustomerContainer> containerRepository,
            IObjectResultProvider objectResultProvider)
        {
            this.logger = logger;
            this.objectResultProvider = objectResultProvider;
            this.containerRepository = containerRepository;
        }

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
                this.logger.LogError(ex, "72A91E25-64D2-44E3-8BFB-AAB2D5C686EE Failed to get existing Venue.");
                return this.objectResultProvider.ServerError();
            }
        }
    }
}
