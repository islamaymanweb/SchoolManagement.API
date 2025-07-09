namespace SchoolManagement.API.DTOs;

public class SubjectWithTeachersDto
{
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = default!;
    public List<TeacherDto> Teachers { get; set; } = new();
}
