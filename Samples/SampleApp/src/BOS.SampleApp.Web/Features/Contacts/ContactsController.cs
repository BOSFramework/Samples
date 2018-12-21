using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BOS.SampleApp.Web.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOS.SampleApp.Web.Features.Contacts
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly IAuthClient _authClient;
        private readonly IDemographicsClient _demoClient;

        public ContactsController(IAuthClient authClient, IDemographicsClient demoClient)
        {
            _authClient = authClient;
            _demoClient = demoClient;
        }

        public async Task<IActionResult> Index()
        {
            var id = new Guid(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var profiles = await _demoClient.GetProfiles();
            var users = await _authClient.GetUsers();

            var model = new ContactsViewModel
            {
                CurrentUser = id,
                Profiles = profiles,
                Users = users
            };

            return View(model);
        }
    }
}