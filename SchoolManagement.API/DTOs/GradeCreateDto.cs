namespace SchoolManagement.API.DTOs;

public class GradeCreateDto
{
    public int StudentId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public int Value { get; set; } // np. 1–6
    public string? Comment { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}