

namespace TicketDepot.Shared
{
    using FluentValidation;

    /// <summary>
    /// The Base JWT Service configuration validator.
    /// </summary>
    public class BaseJwtServiceConfigurationValidator<T> : BaseServiceConfigurationValidator<T>
        where T : IBaseJwtServiceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJwtServiceConfigurationValidator{T}"/> class.
        /// </summary>
        public BaseJwtServiceConfigurationValidator()
            : base()
        {
            this.RuleFor(m => m.SPNClientId).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.SPNClientId)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.Scope).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.Scope)} cannot be empty. Check configuration.");
        }
    }
}
