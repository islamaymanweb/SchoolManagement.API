using SchoolManagement.API.Interfaces;

namespace SchoolManagement.API.Models.Entities;

public class Teacher : IUserEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;

    public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    public ICollection<Schedule> Lessons { get; set; } = new List<Schedule>();
    public ICollection<Class> HomeroomClasses { get; set; } = new List<Class>();
}
