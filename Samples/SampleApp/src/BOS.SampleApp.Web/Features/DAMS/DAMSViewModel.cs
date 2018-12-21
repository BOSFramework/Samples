using BOS.SampleApp.Core.Entities;
using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Features.DAMS
{
    public class DAMSViewModel
    {
        public List<CollectionEntity> Collections { get; set; }
        public Collection NewCollection { get; set; }
    }
}
