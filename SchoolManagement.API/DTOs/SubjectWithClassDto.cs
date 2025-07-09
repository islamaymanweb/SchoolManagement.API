namespace SchoolManagement.API.DTOs;

public class SubjectWithClassDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = null!;
    public int ClassId { get; set; }
    public string ClassName { get; set; } = null!;
}