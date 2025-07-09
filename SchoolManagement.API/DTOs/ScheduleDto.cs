namespace SchoolManagement.API.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }
    public int ClassId { get; set; }
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan Time { get; set; }
}
