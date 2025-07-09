

using SchoolManagement.API.Interfaces;

namespace SchoolManagement.API.Models.Entities;

public class Admin : IUserEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
}