namespace SchoolManagement.API.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }
    public string? ClassName { get; set; }
}