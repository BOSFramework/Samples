using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.Areas.Identity.BOS
{
    public class LoginResponse
    {
        public bool IsVerified { get; set; }
        public Guid? UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
