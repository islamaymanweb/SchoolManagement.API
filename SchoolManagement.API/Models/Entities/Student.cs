using SchoolManagement.API.Interfaces;

namespace SchoolManagement.API.Models.Entities;

public class Student : IUserEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public int? ClassId { get; set; }
    public Class? Class { get; set; } = null!;

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
}