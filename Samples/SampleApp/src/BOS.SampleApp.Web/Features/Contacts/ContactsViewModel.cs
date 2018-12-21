using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOS.SampleApp.Web.Models;

namespace BOS.SampleApp.Web.Features.Contacts
{
    public class ContactsViewModel
    {
        public Guid CurrentUser { get; set; }
        public List<Models.Profile> Profiles { get; set; }
        public List<User> Users { get; set; }
    }
}
