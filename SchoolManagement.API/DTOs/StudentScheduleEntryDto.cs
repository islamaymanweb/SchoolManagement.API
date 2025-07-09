namespace SchoolManagement.API.DTOs;

public class StudentScheduleEntryDto
{
    public string ClassName { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public string TeacherFullName { get; set; } = null!;
    public DayOfWeek DayOfWeek { get; set; }
    public string StartTime { get; set; } = null!;
}
