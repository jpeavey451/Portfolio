
using Microsoft.Extensions.Logging;
using Validation;

namespace TicketDepot.Shared
{
    /// <summary>
    /// Generates token generator classes
    /// </summary>
    public class TokenGeneratorFactory
    {
        private readonly ILogger<TokenGeneratorFactory> logger;

        private readonly IEnumerable<ITokenGenerator> tokenGenerators;

        /// <summary>
        /// Initializes a new instance of <see cref="TokenGeneratorFactory"/>
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="tokenGenerators"></param>
        public TokenGeneratorFactory(
            ILogger<TokenGeneratorFactory> logger,
            IEnumerable<ITokenGenerator> tokenGenerators)
        {
            Requires.NotNull(tokenGenerators, nameof(tokenGenerators));
            this.logger = logger;
            this.tokenGenerators = tokenGenerators;
        }

        /// <summary>
        /// Returns the token generator classes based on Authentication type
        /// </summary>
        /// <returns>ITokenGenerator</returns>
        public ITokenGenerator GetTokenGenerator(string authenticationType)
        {
            Requires.NotNullOrWhiteSpace(authenticationType, nameof(authenticationType));

            ITokenGenerator? selectedTokenGenerator = this.tokenGenerators.FirstOrDefault(e => e.AuthenticationType == authenticationType);

            if (selectedTokenGenerator == null)
            {
                this.logger.LogError(new NotSupportedException($"Invalid authentication type {authenticationType}"), $"ADFB7CE3-7D8B-49D6-86AF-1FBE3C69BA19, Authentication Type {authenticationType} does not have a token generator implemented");
                throw new NotSupportedException($"Invalid authentication type {authenticationType}");
            }

            return selectedTokenGenerator;
        }
    }
}
