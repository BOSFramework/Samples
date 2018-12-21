using BOS.SampleApp.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.HttpClients
{
    public interface IDemographicsClient
    {
        Task<Profile> GetUserProfile(Guid id);
        Task<bool> UpdateProfile(Profile profile);
        Task<List<Profile>> GetProfiles();
    }
}
