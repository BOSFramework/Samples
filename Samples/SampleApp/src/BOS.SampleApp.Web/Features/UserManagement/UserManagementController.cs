using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BOS.SampleApp.Web.Features.Profile;
using BOS.SampleApp.Web.HttpClients;
using BOS.SampleApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BOS.SampleApp.Web.Features.UserManagement
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly IAuthClient _authClient;
        private readonly IDemographicsClient _demoClient;
        private readonly IEmailSender _emailSender;

        public UserManagementController(IAuthClient authClient, IDemographicsClient demoClient, IEmailSender emailSender)
        {
            _authClient = authClient;
            _demoClient = demoClient;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _authClient.GetUsers();
            var model = new UserManagementOverviewViewModel { Users = users };
            return View(model);
        }

        public async Task<IActionResult> AddUser(UserManagementOverviewViewModel model)
        {
            try
            {
                if (model.NewUser.Password != model.NewUser.PasswordConfirmation)
                {
                    return RedirectToAction("Index", "Error");
                }

                var addedUser = await _authClient.AddNewUser(model.NewUser);

                await _emailSender.SendEmailAsync(
                    model.NewUser.Email,
                    "Welcome to BOS",
                    $"<h1>Welcome!</h1><hr /><p>Sign in with your username and password.</p><br /><p>Username: {model.NewUser.Username}, Password: {model.NewUser.Password}</p>");

                return RedirectToAction("Index", "UserManagement");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var userId = new Guid(id);
                var profile = await _demoClient.GetUserProfile(userId);
                var user = await _authClient.GetUserById(userId);
                var roles = await _authClient.GetRoles();
                var assignedRoles = await _authClient.GetUserRolesByUserId(userId);
                var model = new EditUserViewModel
                {
                    Id = userId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    BirthDate = profile.BirthDate,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumbers.FirstOrDefault(),
                    DisplayName = profile.DisplayName,
                    Title = profile.Title,
                    AllRoles = roles,

                    // There is an email property that we get from Auth, but there is also a email property on a 
                    // demographics profile. So this allows flexibility if someone wasn't using BOS Auth to still
                    // maintain email address in our system. However it can also lead to inconsistent data. The
                    // eventual consistency is just something to be aware of. For this app, the email on the 
                    // Auth user is the single source of truth, so we get that here.
                    Email = user.Email,
                    EmailRecord = user.Email
                };

                if (model.BirthDate != null)
                {
                    model.BirthDateString = model.BirthDate.Value.ToString("dd/MM/yyyy");
                }

                foreach(Role r in roles)
                {
                    if (assignedRoles.Any(ro => ro.Id == r.Id))
                    {
                        model.AssignedRoles.Add(r);
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel profileData)
        {
            try
            {
                var profile = new Models.Profile
                {
                    FirstName = profileData.FirstName,
                    LastName = profileData.LastName,
                    Gender = profileData.Gender,
                    Id = profileData.Id,
                    DisplayName = profileData.DisplayName,
                    Title = profileData.Title
                };

                DateTimeOffset formattedDate;

                if (DateTimeOffset.TryParse(profileData.BirthDateString, out formattedDate))
                {
                    profile.BirthDate = formattedDate;
                }
                else
                {
                    profile.BirthDate = profileData.BirthDate;
                }

                if (profileData.PhoneNumber?.Number != null)
                {
                    profile.PhoneNumbers.Add(profileData.PhoneNumber);
                }

                if (profileData.Email != profileData.EmailRecord)
                {
                    var updateEmailSuccess = await _authClient.UpdateUserEmail(profileData.Id, profileData.Email);

                    if (!updateEmailSuccess)
                    {
                        return RedirectToAction("Index", "Error");
                    }
                }

                var success = await _demoClient.UpdateProfile(profile);

                if (success)
                {
                    return RedirectToAction("Index", "UserManagement", new { id = profileData.Id.ToString() });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> AddRole(string roleId, string userId)
        {
            try
            {
                var success = await _authClient.AddRoleToUser(new Guid(roleId), new Guid(userId));

                if (success)
                {
                    return RedirectToAction("Edit", "UserManagement", new { id = userId });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> RevokeRole(string role, string user)
        {
            try
            {
                var roleId = new Guid(role);
                var userId = new Guid(user);

                var success = await _authClient.RevokeRole(roleId, userId);

                if (success)
                {
                    return RedirectToAction("Edit", "UserManagement", new { id = user });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                var id = new Guid(userId);

                var success = await _authClient.DeleteUser(id);

                if (success)
                {
                    return RedirectToAction("Index", "UserManagement");
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> ResetPassword(EditUserViewModel modelData)
        {
            try
            {
                var success = await _authClient.ForcePasswordChange(modelData.Id, modelData.NewPassword);

                if (success)
                {
                    return RedirectToAction("Index", "UserManagement");
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