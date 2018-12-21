using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BOS.SampleApp.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BOS.SampleApp.Web.HttpClients
{
    public class DAMSClient : IDAMSClient
    {
        private readonly HttpClient _httpClient;

        public DAMSClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddAsset(Asset asset, Guid collectionId)
        {
            var payload = new
            {
                id = asset.Id,
                description = asset.Description,
                name = asset.Name,
                fileExtension = asset.FileExtension,
                url = asset.URL
            };
            var response = await _httpClient.PostAsJsonAsync("Assets?api-version=1.0", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddAssetToCollection(Guid assetId, Guid collectionId)
        {
            var payload = new { assetId };
            var response = await _httpClient.PostAsJsonAsync($"Collections({collectionId.ToString()})/AddAssetToCollection?api-version=1.0", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddCollection(Collection collection)
        {
            var payload = new { id = collection.Id, name = collection.Name, createdBy = collection.CreatedBy, createdOn = collection.CreatedOn,
                               description = collection.Description };
            var response = await _httpClient.PostAsJsonAsync("Collections?api-version=1.0", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<Collection> GetCollectionById(Guid collectionId)
        {
            var response = await _httpClient.GetAsync($"Collections({collectionId.ToString()})?$expand=Assets&api-version=1.0");
            var jsonCollection = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            var collection = JsonConvert.DeserializeObject<Collection>(jsonCollection.ToString());
            return collection;
        }

        public async Task<bool> RemoveAssetFromCollection(Guid assetId, Guid collectionId)
        {
            var payload = new { assetId };
            var response = await _httpClient.PostAsJsonAsync($"Collections({collectionId.ToString()})/RemoveAssetFromCollection", payload);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }
    }
}
