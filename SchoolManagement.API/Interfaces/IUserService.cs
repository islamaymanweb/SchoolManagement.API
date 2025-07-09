using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.API.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> AddNewUserAsync(AddUserModel addUserModel);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<UserDto?> GetUserByIdAsync(string id);
        Task<PagedUsers> GetUsersPaged(PagedRequest request);
        Task<List<Role>> GeRolesAsync();
        Task<bool> UpdateUserAsync(UpdateUser updateUser);
    }
}