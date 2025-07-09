namespace SchoolManagement.API.Models.Entities;

public class ClassSubject
{
    public int Id { get; set; }

    public int ClassId { get; set; }
    public Class Class { get; set; } = default!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = default!;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = default!;
}