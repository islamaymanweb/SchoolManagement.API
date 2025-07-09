namespace SchoolManagement.API.DTOs;

public class TeacherScheduleEntryDto
{
    public int Id { get; set; }
    public string ClassName { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public DayOfWeek DayOfWeek { get; set; }
    public string StartTime { get; set; } = null!;
}
