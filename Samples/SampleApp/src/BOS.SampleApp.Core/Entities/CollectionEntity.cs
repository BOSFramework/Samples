using System;
using System.Collections.Generic;
using System.Text;

namespace BOS.SampleApp.Core.Entities
{
    public class CollectionEntity
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
    }
}
