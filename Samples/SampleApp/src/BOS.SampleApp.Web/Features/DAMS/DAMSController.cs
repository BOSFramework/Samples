using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BOS.SampleApp.Core.Entities;
using BOS.SampleApp.Core.Interfaces;
using BOS.SampleApp.Web.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOS.SampleApp.Web.Features.DAMS
{
    [Authorize]
    public class DAMSController : Controller
    {
        private readonly IDAMSClient _damsClient;
        private readonly IDAMSRepository _damsRepository;

        public DAMSController(IDAMSClient damsClient, IDAMSRepository damsRepository)
        {
            _damsClient = damsClient;
            _damsRepository = damsRepository;
        }

        public IActionResult Index()
        {
            try
            {
                var id = new Guid(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var collectionEntities = _damsRepository.GetCollectionsByOwnerId(id);
                var model = new DAMSViewModel { Collections = collectionEntities };
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> Collection(string id)
        {
            try
            {
                var collection = await _damsClient.GetCollectionById(new Guid(id));
                var model = new CollectionViewModel
                {
                    Collection = collection
                };
                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> AddCollection(DAMSViewModel inputData)
        {
            try
            {
                var collection = inputData.NewCollection;
                collection.Id = Guid.NewGuid();
                collection.CreatedOn = DateTimeOffset.Now.ToString();
                collection.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();
                var success = await _damsClient.AddCollection(collection);

                if (success)
                {
                    _damsRepository.AddCollection(new CollectionEntity
                    {
                        Id = collection.Id,
                        Name = collection.Name,
                        OwnerId = new Guid(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)
                    });

                    return RedirectToAction("Index", "DAMS");
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> AddAsset(CollectionViewModel inputData)
        {
            try
            {
                var asset = inputData.NewAsset;
                asset.Id = Guid.NewGuid();
                var success = await _damsClient.AddAsset(asset, inputData.Collection.Id);
                bool addToCollectionSuccess = false;

                if (success)
                {
                    addToCollectionSuccess = await _damsClient.AddAssetToCollection(asset.Id, inputData.Collection.Id);
                }

                if (addToCollectionSuccess)
                {
                    return RedirectToAction("Collection", "DAMS", new { id = inputData.Collection.Id.ToString() });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> RemoveAsset(string asset, string collection)
        {
            try
            {
                var assetId = new Guid(asset);
                var collectionId = new Guid(collection);
                var success = await _damsClient.RemoveAssetFromCollection(assetId, collectionId);

                if (success)
                {
                    return RedirectToAction("Collection", "DAMS", new { id = collection });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}