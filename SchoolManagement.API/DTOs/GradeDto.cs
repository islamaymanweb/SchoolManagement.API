namespace SchoolManagement.API.DTOs;

public class GradeDto
{
    public string StudentName { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public int Value { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}

public class PagedGrades
{
    public int TotalRecords { get; set; }
    public List<GradeDto> Data { get; set; } = new List<GradeDto>();
}