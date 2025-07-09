namespace SchoolManagement.API.DTOs;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<string> AssignmentsDto { get; set; } = [];
    public List<SubjectAssignmentDto> Assignments { get; set; } = new();

}

public class PagedSubjects
{
    public int TotalRecords { get; set; }
    public List<SubjectDto> Data { get; set; } = [];
}
