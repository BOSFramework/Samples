using BOS.SampleApp.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.HttpClients
{
    public class DemographicsClient : IDemographicsClient
    {
        private readonly HttpClient _httpClient;

        public DemographicsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Profile>> GetProfiles()
        {
            var response = await _httpClient.GetAsync("People?api-version=1.0");
            var jsonProfiles = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var profiles = JsonConvert.DeserializeObject<List<Profile>>(jsonProfiles["value"].ToString());

            if (profiles == null)
            {
                return new List<Profile>();
            }

            return profiles;
        }

        public async Task<Profile> GetUserProfile(Guid id)
        {
            var response = await _httpClient.GetAsync($"People({id.ToString()})?api-version=1.0");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Profile();
            }

            var profile = JsonConvert.DeserializeObject<Profile>(response.Content.ReadAsStringAsync().Result);
            return profile;
        }

        public async Task<bool> UpdateProfile(Profile profile)
        {
            var response = await _httpClient.PutAsJsonAsync($"People({profile.Id.ToString()})?api-version=1.0", profile);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            // If everything wasn't ok then something bad happened
            return false;
        }
    }
}
