using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.HttpClients
{
    public interface IDAMSClient
    {
        Task<bool> AddCollection(Collection collection);
        Task<bool> AddAsset(Asset asset, Guid collectionId);
        Task<bool> AddAssetToCollection(Guid assetId, Guid collectionId);
        Task<bool> RemoveAssetFromCollection(Guid assetId, Guid collectionId);
        Task<Collection> GetCollectionById(Guid collectionId);
    }
}
