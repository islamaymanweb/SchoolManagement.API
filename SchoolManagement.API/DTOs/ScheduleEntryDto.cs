namespace SchoolManagement.API.DTOs;

public class ScheduleEntryDto
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public string StartTime { get; set; } = null!;

    public int ClassId { get; set; }
    public string? ClassName { get; set; }

    public int SubjectId { get; set; }
    public string? SubjectName { get; set; }

    public int TeacherId { get; set; }
    public string? TeacherFullName { get; set; }
}