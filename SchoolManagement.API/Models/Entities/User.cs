using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.API.Models.Entities;

public class User : IdentityUser
{
    public bool IsActive { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime? LastSuccessfulLogin { get; set; }

    public int? AdminId { get; set; }
    public Admin? Admin { get; set; }
    
    public int? TeacherId { get; set; }
    public Teacher? Teacher { get; set; }

    public int? StudentId { get; set; }
    public Student? Student { get; set; }
}
