namespace SchoolManagement.API.Models.Entities;

public class Subject
{
    public int Id { get; set; }
    public string Name { get; set; } = null!; // np. Matematyka
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
