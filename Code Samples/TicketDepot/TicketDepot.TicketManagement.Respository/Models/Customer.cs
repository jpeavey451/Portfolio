using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.TicketManagement.Repository
{
    /// <summary>
    /// The Customer class.
    /// </summary>
    [JsonObject(
        Description = "Customer Document class.",
        ItemTypeNameHandling = TypeNameHandling.None,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ItemNullValueHandling = NullValueHandling.Ignore)]
    public class Customer
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Customer"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountNumber"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        public Customer(string id, string accountNumber, string firstName, string lastName)
        {
            Id = id;
            AccountNumber = accountNumber;
            FirstName = firstName;
            LastName = lastName;
        }

        [Required]
        [JsonProperty("id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }
}
