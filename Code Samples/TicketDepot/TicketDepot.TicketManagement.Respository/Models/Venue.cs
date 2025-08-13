using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The Venue class.
    /// </summary>
    [JsonObject(
        Description = "Venue Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Venue : BaseVenue
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Venue"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="capacity"></param>
        public Venue(string name, string description, int maxCapacity)
            :base(name, description)
        {
            this.Id = Guid.NewGuid().ToString("N");
            this.CreatedDate = DateTimeOffset.UtcNow;
            this.MaxCapacity = maxCapacity;
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("_etag")]
        public string? Etag { get; set; }

        [JsonProperty("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }

        [Range(1,100000)]
        public int MaxCapacity { get; set; }
    }
}
