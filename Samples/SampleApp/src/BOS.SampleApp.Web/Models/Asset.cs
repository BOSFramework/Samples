using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Models
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public string MIMEType { get; set; }
        public float Size { get; set; }
        public string ThumbnailURL { get; set; }
        public string FileExtension { get; set; }
        public List<Extension> Extensions { get; set; }
    }
}
