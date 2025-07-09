namespace SchoolManagement.API.DTOs;

public class LessonForAttendanceDto
{
    public int ScheduleId { get; set; }
    public string SubjectName { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string StartTime { get; set; } = null!;
    public bool HasAttendance { get; set; }
}
