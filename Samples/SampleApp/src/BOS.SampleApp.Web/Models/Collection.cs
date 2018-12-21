using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Models
{
    public class Collection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public List<Asset> Assets { get; set; }

        public Collection()
        {
            Assets = new List<Asset>();
        }
    }
}
