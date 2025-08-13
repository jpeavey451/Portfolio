
namespace TicketDepot.Shared
{
    public class ApiToken
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApiToken"/>.
        /// </summary>
        /// <param name="appRegistrationScope"></param>
        /// <param name="tokenValue"></param>
        /// <param name="validTo"></param>
        public ApiToken(string appRegistrationScope, string tokenValue, DateTime validTo)
        { 
            this.AppRegistrationScope = appRegistrationScope;
            this.TokenValue = tokenValue;
            this.ValidTo = validTo;
        }

        /// <summary>
        /// Gets or Sets the api scope.
        /// </summary>
        public string AppRegistrationScope { get; set; }

        /// <summary>
        /// Gets or Sets the bearer token.
        /// </summary>
        public string TokenValue { get; set; }

        /// <summary>
        /// Gets or Sets the token expiration time.
        /// </summary>
        public DateTime ValidTo { get; set; }
    }
}
