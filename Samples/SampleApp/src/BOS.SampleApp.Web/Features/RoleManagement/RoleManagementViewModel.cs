using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Features.RoleManagement
{
    public class RoleManagementViewModel
    {
        public List<Role> Roles { get; set; }
        public string NewRoleName { get; set; }
    }
}
