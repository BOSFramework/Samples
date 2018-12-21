using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOS.SampleApp.Web.HttpClients;
using Microsoft.AspNetCore.Mvc;
using BOS.SampleApp.Web.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BOS.SampleApp.Web.Features.Profile
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IDemographicsClient _demoClient;
        private readonly IAuthClient _authClient;

        public ProfileController(IDemographicsClient demoClient, IAuthClient authClient)
        {
            _demoClient = demoClient;
            _authClient = authClient;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var id = new Guid(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                var profile = await _demoClient.GetUserProfile(id);
                var userEmail = _authClient.GetUserById(id).Result.Email;
                var model = new ProfileViewModel
                {
                    Id = id,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Birthdate = profile.BirthDate,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumbers.FirstOrDefault(),
                    DisplayName = profile.DisplayName,
                    Title = profile.Title,
                    Email = userEmail,
                    EmailRecord = userEmail
                };

                if (model.Birthdate != null)
                {
                    model.BirthDateString = model.Birthdate.Value.ToString("dd/MM/yyyy");
                }

                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> Update(ProfileViewModel profileData)
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
                    Title = profileData.Title,
                };

                DateTimeOffset formattedDate;

                if (DateTimeOffset.TryParse(profileData.BirthDateString, out formattedDate))
                {
                    profile.BirthDate = formattedDate;
                }
                else
                {
                    profile.BirthDate = profileData.Birthdate;
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
                    return RedirectToAction("Index", "Profile", new { id = profileData.Id.ToString() });
                }

                return RedirectToAction("Index", "Error");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<IActionResult> Details(string id)
        {
            try
            {
                var userId = new Guid(id);
                var user = await _authClient.GetUserById(userId);
                var profile = await _demoClient.GetUserProfile(userId);
                var model = new ProfileViewModel
                {
                    Id = userId,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                    Birthdate = profile.BirthDate,
                    Gender = profile.Gender,
                    PhoneNumber = profile.PhoneNumbers.FirstOrDefault(),
                    DisplayName = profile.DisplayName,
                    Title = profile.Title,
                    Email = user.Email
                };

                if (model.Birthdate != null)
                {
                    model.BirthDateString = model.Birthdate.Value.ToString("dd/MM/yyyy");
                }

                return View(model);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}