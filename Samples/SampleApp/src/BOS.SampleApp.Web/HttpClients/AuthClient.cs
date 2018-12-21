using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BOS.SampleApp.Web.Areas.Identity.BOS;
using BOS.SampleApp.Web.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BOS.SampleApp.Web.HttpClients
{
    public class AuthClient : IAuthClient
    {
        private readonly HttpClient _httpClient;

        public AuthClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<User> AddNewUser(UserCreationInput newUser)
        {
            var payload = new { username = newUser.Username, password = newUser.Password, email = newUser.Email };
            var response = await _httpClient.PostAsJsonAsync("Users?api-version=1.0", payload).ConfigureAwait(false);
            
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Invalid Input");
            }

            var user = JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
            return user;
        }

        public async Task<bool> AddRole(string roleName)
        {
            var response = await _httpClient.PostAsync($"Roles?api-version=1.0&name={roleName}", null);
            
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddRoleToUser(Guid roleId, Guid userId)
        {
            var payload = new { roleId };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/AssignRole?api-version=1.0", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else return false;
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var response = await _httpClient.DeleteAsync($"Users({userId.ToString()})");

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ForcePasswordChange(Guid id, string newPassword)
        {
            var payload = new { newPassword };
            var response = await _httpClient.PostAsJsonAsync($"Users({id.ToString()})/ForcePasswordChange?api-version=1.0", payload);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<List<Role>> GetRoles()
        {
            var response = await _httpClient.GetAsync($"Roles?api-version=1.0");
            var rolesJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var roles = JsonConvert.DeserializeObject<List<Role>>(rolesJson["value"].ToString());
            return roles;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var response = await _httpClient.GetAsync($"Users?$filter=Email eq '{email.ToString()}'&api-version=1.0");
            var usersJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson["value"].ToString());
            
            if (users.Count == 0)
            {
                return null;
            }

            return users[0];
        }

        public async Task<User> GetUserById(Guid userId)
        {
            var response = await _httpClient.GetAsync($"Users({userId.ToString()})?api-version=1.0&");
            return JsonConvert.DeserializeObject<User>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<List<Role>> GetUserRolesByUserId(Guid userId)
        {
            var response = await _httpClient.GetAsync($"UserRoles({userId.ToString()})?api-version=1.0");
            var rolesJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var roles = JsonConvert.DeserializeObject<List<Role>>(rolesJson["value"].ToString());
            return roles;
        }

        public async Task<List<User>> GetUsers()
        {
            var response = await _httpClient.GetAsync("Users?$filter=Deleted eq false&api-version=1.0");
            var usersJson = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var users = JsonConvert.DeserializeObject<List<User>>(usersJson["value"].ToString());
            return users;
        }

        public async Task<bool> RevokeRole(Guid roleId, Guid userId)
        {
            var payload = new { roleId };
            var response = await _httpClient.PostAsJsonAsync($"Users({userId.ToString()})/RevokeRole", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public async Task<LoginResponse> SignInAsync(string username, string password)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new { username, password };
            var response = await _httpClient.PostAsJsonAsync("Verification?api-version=1.0", payload).ConfigureAwait(false);
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Content.ReadAsStringAsync().Result);
            return loginResponse;
        }

        public async Task<bool> UpdateUserEmail(Guid id, string email)
        {
            var payload = new { email };
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_httpClient.BaseAddress}Users({id.ToString()})?api-version=1.0");
            request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
