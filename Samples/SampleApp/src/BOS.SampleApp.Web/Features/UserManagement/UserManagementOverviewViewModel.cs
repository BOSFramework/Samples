using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Features.UserManagement
{
    public class UserManagementOverviewViewModel
    {
        public List<User> Users { get; set; }
        public UserCreationInput NewUser { get; set; }
    }
}
