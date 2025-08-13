

namespace TicketDepot.Shared
{
    /// <summary>
    /// The base dependent service configuration interface.
    /// </summary>
    public interface IBaseServiceConfiguration
    {
        /// <summary>
        /// Section Name as defined in Settings.xml
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// API Version of Claims Service.
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// Base URl of Claims Service APIs.
        /// </summary>
        public string BaseAddress { get; set; }
    }
}
