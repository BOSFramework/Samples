using BOS.SampleApp.Core.Entities;
using BOS.SampleApp.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOS.SampleApp.Infrastructure.Data
{
    public class DAMSRepository : IDAMSRepository
    {
        private readonly DAMSContext _context;

        public DAMSRepository(DAMSContext context)
        {
            _context = context;
        }

        public void AddCollection(CollectionEntity collection)
        {
            _context.Collections.Add(collection);
            _context.SaveChanges();
        }

        public List<CollectionEntity> GetCollectionsByOwnerId(Guid ownerId)
        {
            List<CollectionEntity> collections = new List<CollectionEntity>();

            foreach (CollectionEntity c in _context.Collections.Where(c => c.OwnerId == ownerId))
            {
                collections.Add(c);
            }

            _context.SaveChanges();
            return collections;
        }
    }
}
