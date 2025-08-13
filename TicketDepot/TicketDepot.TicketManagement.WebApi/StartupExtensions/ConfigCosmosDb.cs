using Microsoft.Azure.Cosmos;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// The CosmosDb configuration class.
    /// </summary>
    public static class ConfigCosmosDb
    {
        /// <summary>
        /// Adds class for cosmos db.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCosmosDbClients(this IServiceCollection services, IConfiguration configuration)
        {
            ICosmosClient cosmosDbClient = CreateCosmosDbClient(configuration);
            services.AddSingleton(typeof(ICosmosClient), cosmosDbClient);
            services.AddSingleton((IEventContainer)CreateCosmosContainerClient<EventContainer>(cosmosDbClient, ContainerConfig.EventContainerName));
            services.AddSingleton((IReservationContainer)CreateCosmosContainerClient<ReservationContainer>(cosmosDbClient, ContainerConfig.ReservationContainerName));
            services.AddSingleton((ITicketTypeContainer)CreateCosmosContainerClient<TicketTypeContainer>(cosmosDbClient, ContainerConfig.TicketTypeContainerName));
            services.AddSingleton((IVenueContainer)CreateCosmosContainerClient<VenueContainer>(cosmosDbClient, ContainerConfig.VenueContainerName));
            services.AddSingleton((ICustomerContainer)CreateCosmosContainerClient<CustomerContainer>(cosmosDbClient, ContainerConfig.CustomerContainerName));
            services.AddSingleton((ITransactionContainer)CreateCosmosContainerClient<TransactionContainer>(cosmosDbClient, ContainerConfig.TransactionContainerName));

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IVenueRepository, VenueRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ICustomerRespository, CustomerRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IContainerRepository<IEventContainer>, ContainerRepository<IEventContainer>>();
            services.AddScoped<IContainerRepository<IReservationContainer>, ContainerRepository<IReservationContainer>>();
            services.AddScoped<IContainerRepository<ITicketTypeContainer>, ContainerRepository<ITicketTypeContainer>>();
            services.AddScoped<IContainerRepository<IVenueContainer>, ContainerRepository<IVenueContainer>>();
            services.AddScoped<IContainerRepository<ICustomerContainer>, ContainerRepository<ICustomerContainer>>();
            services.AddScoped<IContainerRepository<ITransactionContainer>, ContainerRepository<ITransactionContainer>>();
        }


        /// <summary>
        /// This method creates the singleton <see cref="CosmosDbClient"/>.
        /// </summary>
        /// <param name="configuration">The current configuration <see cref="IConfiguration"/>.</param>
        /// <returns>The created instance of <see cref="ICosmosClient"/>.</returns>
        private static ICosmosClient CreateCosmosDbClient(IConfiguration configuration)
        {
            IConfigurationSection dbConfigurations = configuration.GetSection(ConfigSections.DbConfigurationSection);
            string databaseName = dbConfigurations.GetSection(ConfigSections.DatabaseNameConfigurationSection)?.Value!;
            string account = dbConfigurations.GetSection(ConfigSections.DatabaseNameAccountSection)?.Value!;
            string key = dbConfigurations.GetSection(ConfigSections.DatabaseNameKeySections)?.Value!;

            var cosmosClient = new CosmosClient(account, key, new CosmosClientOptions() { ConnectionMode = ConnectionMode.Gateway });
            var database = cosmosClient.GetDatabase(databaseName);
            ICosmosClient cosmosDbClient = new CosmosDbClient() { CosmosClientInstance = cosmosClient, CosmosDatabase = database };

            return cosmosDbClient;
        }

        /// <summary>
        /// Creates the container specific class instance.
        /// </summary>
        /// <typeparam name="T">The type of container class to create <see cref="ICosmosContainer"/>.</typeparam>
        /// <param name="cosmosDbClient">The singleton <see cref="ICosmosClient"/>.</param>
        /// <param name="containerName">The name of the container to encapsulate.</param>
        /// <returns>The instance of <see cref="ICosmosContainer"/> created.</returns>
        private static ICosmosContainer CreateCosmosContainerClient<T>(ICosmosClient cosmosDbClient, string containerName)
            where T : ICosmosContainer, new()
        {
            if (cosmosDbClient is null)
            {
                throw new InvalidOperationException("D2A64307-2C25-4F94-94CE-21D547286569 Unexpected Error. CosmosDbClient is null.");
            }

            if (cosmosDbClient.CosmosDatabase is null)
            {
                throw new InvalidOperationException("FEC0358C-E3F9-478E-BA1D-0F7340523322 Unexpected Error. CosmosDbClient is null.");
            }

            T container = new T
            {
                Container = cosmosDbClient.CosmosDatabase.GetContainer(containerName),
            };

            return container;
        }
    }
}
