using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Models
{
    public class Profile
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("phoneNumbers")]
        public List<PhoneNumber> PhoneNumbers { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("birthDate")]
        public DateTimeOffset? BirthDate { get; set; }

        public Profile()
        {
            PhoneNumbers = new List<PhoneNumber>();
            Addresses = new List<Address>();
        }
    }
}
