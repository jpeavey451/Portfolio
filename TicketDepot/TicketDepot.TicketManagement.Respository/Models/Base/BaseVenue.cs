using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The base Venue class.
    /// </summary>
    public class BaseVenue
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BaseVenue"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="capacity"></param>
        public BaseVenue(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
