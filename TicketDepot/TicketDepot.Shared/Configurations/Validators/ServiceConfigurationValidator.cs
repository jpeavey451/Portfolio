
using FluentValidation;

namespace TicketDepot.Shared
{
    /// <summary>
    /// Validates <see cref="ServiceConfigurationValidator"/> configuration object.
    /// </summary>
    public class ServiceConfigurationValidator : AbstractValidator<IServiceAuthorizationConfiguration>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthorizationConfigurationValidator"/> class.
        /// This is the validation class for the Authorization  configuration.
        /// </summary>
        public ServiceConfigurationValidator()
        {
            this.RuleFor(m => m.Instance).NotEmpty().WithMessage(m => $"{IServiceAuthorizationConfiguration.SectionName}.{nameof(m.Instance)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.ClientId).NotEmpty().WithMessage(m => $"{IServiceAuthorizationConfiguration.SectionName}.{nameof(m.ClientId)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.SPNName).NotEmpty().WithMessage(m => $"{IServiceAuthorizationConfiguration.SectionName}.{nameof(m.SPNName)} cannot be empty. Check configuration.");

            this.RuleFor(m => m.Instance)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Instance))
                .WithMessage(m => $"Invalid URL {IServiceAuthorizationConfiguration.SectionName}.{nameof(m.Instance)}={m.Instance}");

            this.RuleFor(m => m.KeyVaultURL)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.KeyVaultURL))
                .WithMessage(m => $"Invalid URL {IServiceAuthorizationConfiguration.SectionName}.{nameof(m.KeyVaultURL)}={m.KeyVaultURL}");
        }
    }
}
