using BOS.SampleApp.Web.Areas.Identity.BOS;
using BOS.SampleApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOS.SampleApp.Web.HttpClients
{
    public interface IAuthClient
    {
        Task<LoginResponse> SignInAsync(string username, string password);
        Task<List<User>> GetUsers();
        Task<User> AddNewUser(UserCreationInput newUser);
        Task<List<Role>> GetUserRolesByUserId(Guid userId);
        Task<List<Role>> GetRoles();
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(Guid userId);
        Task<bool> DeleteUser(Guid userId);
        Task<bool> AddRoleToUser(Guid roleId, Guid userId);
        Task<bool> RevokeRole(Guid roleId, Guid userId);
        Task<bool> AddRole(string roleName);
        Task<bool> ForcePasswordChange(Guid id, string newPassword);
        Task<bool> UpdateUserEmail(Guid id, string email);
    }
}
