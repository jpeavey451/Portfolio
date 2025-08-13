
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using TicketDepot.Shared;
using Validation;

namespace TicketDepot.TicketManagement.WebApi
{
    /// <summary>
    /// Configuration Startup class
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Gets the configuration for the AzureAd and validates it.
        /// </summary>
        /// <param name="builder">The <see cref="WebApplicationBuilder"/>.</param>
        public static void AddAndValidateAuthorizationConfiguration(this WebApplicationBuilder builder)
        {
            IConfigurationSection defaultConfiguration = builder.Configuration.GetSection(AuthConfig.AzureADSectionName);
            builder.Services.AddSingleton<AbstractValidator<IServiceAuthorizationConfiguration>, ServiceConfigurationValidator>();

            builder.Services.Configure<ServiceAuthorizationConfiguration>(defaultConfiguration)
                .TryAddSingleton<IServiceAuthorizationConfiguration>(serviceProvider =>
            {
                ServiceAuthorizationConfiguration serviceConfig = serviceProvider.GetService<IOptions<ServiceAuthorizationConfiguration>>()!.Value;
                AbstractValidator<IServiceAuthorizationConfiguration> validator = serviceProvider.GetService<AbstractValidator<IServiceAuthorizationConfiguration>>()!;
                validator.ValidateAndThrow(serviceConfig);

                // only to be used with KeyVault, given the cost, not viable for a personal POC
                //builder.AddKeyVaultSecerts(serviceConfig);

                return serviceConfig;
            });
        }

        /// <summary>
        /// Extension to add and validate configs
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="config">Config</param>
        public static void AddAndValidateConfigurations(this IServiceCollection services, IConfiguration config)
        {
            Requires.NotNull(services, nameof(services));
            Requires.NotNull(config, nameof(config));

            services.AddAndValidateConfigurationObject<
                IPaymentServiceConfiguration,
                PaymentServiceConfiguration,
                PaymentServiceConfigurationValidator>(config, AuthConfig.PaymentServiceSectionName);
        }

        /// <summary>
        /// Adds the specified ASP.NET Core configuration object to the service collection, and runs the associated validation.
        /// </summary>
        /// <typeparam name="TI">The interface for the configuration object.</typeparam>
        /// <typeparam name="TC">The class that implements interface TI.</typeparam>
        /// <typeparam name="TV">THe class that validates the configuration object</typeparam>
        /// <param name="services">Where the configuration object will be added.</param>
        /// <param name="config">The reference to all configs.</param>
        /// <param name="sectionName">The name of the section in the config. Should be retrieved from TI.SectionName.</param>
        private static void AddAndValidateConfigurationObject<TI, TC, TV>(this IServiceCollection services, IConfiguration config, string sectionName)
            where TI : class
            where TC : class, TI
            where TV : AbstractValidator<TI>
        {
            services.AddSingleton<AbstractValidator<TI>, TV>();
            services.Configure<TC>(config.GetSection(sectionName))
                .TryAddSingleton<TI>(serviceProvider =>
                {
                    TC config = serviceProvider.GetService<IOptions<TC>>()!.Value;
                    AbstractValidator<TI> validator = serviceProvider.GetService<AbstractValidator<TI>>()!;
                    validator.ValidateAndThrow(config);
                    return config;
                });
        }

        /// <summary>
        /// Adds Active/Enabled given secrets into IConfiguration from given keyvault
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceConfig"></param>
        private static void AddKeyVaultSecerts(this WebApplicationBuilder builder, IServiceAuthorizationConfiguration serviceConfig )
        {
            if ( !TimeSpan.TryParse( serviceConfig.ReloadInterval, out TimeSpan spnSecretRefreshIntervalTimeSpan ) )
            {
                spnSecretRefreshIntervalTimeSpan = new TimeSpan( 0, 30, 00 );
            }

            // Reloading at 30 min interval to ensure only Enabled and Active (not expired) Secrets are available in IConfiguration.
            builder.Configuration.AddAzureKeyVault(
                new Uri(serviceConfig.KeyVaultURL!),
                new DefaultAzureCredential(),
                new AzureKeyVaultConfigurationOptions
                {
                    ReloadInterval = spnSecretRefreshIntervalTimeSpan,
                    Manager = new ActiveKeyVaultSecretManager([serviceConfig.SPNName!])
                } );
        }
}
