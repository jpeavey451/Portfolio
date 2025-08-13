// Copyright (c) Volkswagen Group. All rights reserved.

namespace TicketDepot.Shared
{
    using System.Threading.Tasks;

    /// <summary>
    /// The interface for <see cref="TokenStore"/>.
    /// </summary>
    public interface ITokenStore
    {
        /// <summary>
        /// Gets a bearer token for an API for a specific sope.
        /// We may need to discuss whether to extend this for multiple scopes for an api.
        /// </summary>
        /// <param name="scopeType"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        Task<string> GetAccessTokenAsync(ServiceType scopeType, string scope);
    }
}