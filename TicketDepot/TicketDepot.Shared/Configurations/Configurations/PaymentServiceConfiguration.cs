
using System.Diagnostics.CodeAnalysis;

namespace TicketDepot.Shared
{
    /// <summary>
    /// The Payment Service Configuration.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PaymentServiceConfiguration : BaseJwtServiceConfiguration, IPaymentServiceConfiguration
    {
        /// <inheritdoc/>
        public override string SectionName => AuthConfig.PaymentServiceSectionName;

        /// <inheritdoc/>
        public override string Scope => $"api://{this.SPNClientId}/.default";
    }
}
