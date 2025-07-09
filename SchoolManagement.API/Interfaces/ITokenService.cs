using SchoolManagement.API.Models.Entities;

namespace backend.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtTokenForUser(User user);
    }
}