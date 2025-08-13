
namespace TicketDepot.Shared
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for <see cref="TokenGenerator"/>.
    /// </summary>
    public interface ITokenGenerator
    {
        /// <summary>
        /// Defines the authentication type. Ex Auth0, Azure AD
        /// </summary>
        /// <returns>AuthenticationTypen</returns>
        string AuthenticationType { get; }

        /// <summary>
        /// Get access token based on the specified scope.
        /// </summary>
        /// <returns>Access token</returns>
        Task<string> GetAccessTokenAsync(string scope);
    }
}