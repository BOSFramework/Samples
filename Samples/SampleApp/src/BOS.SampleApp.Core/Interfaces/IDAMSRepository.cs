using BOS.SampleApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.SampleApp.Core.Interfaces
{
    public interface IDAMSRepository
    {
        void AddCollection(CollectionEntity collection);
        List<CollectionEntity> GetCollectionsByOwnerId(Guid ownerId);
    }
}
