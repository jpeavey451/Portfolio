
using FluentValidation;

namespace TicketDepot.Shared
{

    /// <summary>
    /// The Base API Key Service configuration validator.
    /// </summary>
    public class BaseApiKeyServiceConfigurationValidator<T> : BaseServiceConfigurationValidator<T>
        where T : IBaseApiKeyServiceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiKeyServiceConfigurationValidator{T}"/> class.
        /// </summary>
        public BaseApiKeyServiceConfigurationValidator()
            : base()
        {
            this.RuleFor(m => m.ApiKeyPrimary).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.ApiKeyPrimary)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.ApiKeySecondary).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.ApiKeySecondary)} cannot be empty. Check configuration.");
        }
    }
}
