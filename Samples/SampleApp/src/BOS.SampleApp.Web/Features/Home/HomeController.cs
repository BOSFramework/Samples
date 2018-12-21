using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOS.SampleApp.Web.Features.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                Username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value
            };

            return View(model);
        }
    }
}