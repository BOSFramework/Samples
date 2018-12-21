using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOS.SampleApp.Web.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOS.SampleApp.Web.Features.RoleManagement
{
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private readonly IAuthClient _authClient;

        public RoleManagementController(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _authClient.GetRoles();
            var model = new RoleManagementViewModel
            {
                Roles = roles,
                NewRoleName = null
            };

            return View(model);
        }

        public async Task<IActionResult> Remove(string roleId)
        {
            //  todo:
            //      Add functionality to remove roles when its in auth
            return Ok();
        }

        public async Task<IActionResult> Add(RoleManagementViewModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.NewRoleName))
                {
                    return BadRequest();
                }

                var success = await _authClient.AddRole(model.NewRoleName);

                if (success)
                {
                    return RedirectToAction("Index", "RoleManagement");
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}