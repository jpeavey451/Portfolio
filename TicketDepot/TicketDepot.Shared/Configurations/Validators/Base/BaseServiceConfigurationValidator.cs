

namespace TicketDepot.Shared
{
    using FluentValidation;
    using System;

    /// <summary>
    /// The Base dependent Service configuration validator.
    /// </summary>
    public class BaseServiceConfigurationValidator<T> : AbstractValidator<T>
        where T : IBaseServiceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseServiceConfigurationValidator{T}"/> class.
        /// </summary>
        public BaseServiceConfigurationValidator()
        {
            this.RuleFor(m => m.SectionName).NotEmpty().WithMessage(m => $"{typeof(T).FullName}.{nameof(m.SectionName)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.ApiVersion).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.ApiVersion)} cannot be empty. Check configuration.");
            this.RuleFor(m => m.BaseAddress).NotEmpty().WithMessage(m => $"{m.SectionName}.{nameof(m.BaseAddress)} cannot be empty. Check configuration.");

            this.RuleFor(m => m.BaseAddress)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.BaseAddress))
                .WithMessage(m => $"Invalid URL {m.SectionName}.{nameof(m.BaseAddress)}={m.BaseAddress}");
        }
    }
}
