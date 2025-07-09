namespace SchoolManagement.API.DTOs;

public class ClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? HomeroomTeacherId { get; set; }
    public string? HomeroomTeacherName { get; set; }
    public int? StudentCount { get; set; }
    public List<int> AssignedStudentIds { get; set; } = new List<int>();
}

public class PagedClasses
{
    public int TotalRecords { get; set; }
    public List<ClassDto> Data { get; set; } = new List<ClassDto>();
}