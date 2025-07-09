namespace SchoolManagement.API.Models.Entities;

public class Class
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int? HomeroomTeacherId { get; set; }
    public Teacher? HomeroomTeacher { get; set; }

    public ICollection<ClassSubject> ClassSubjects { get; set; } = new List<ClassSubject>();
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}