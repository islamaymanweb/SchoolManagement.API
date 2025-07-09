
namespace SchoolManagement.API.Models.Entities;

public class Attendance
{
    public int Id { get; set; }

    public string? Comment { get; set; }
    public string? ModifiedByUserId { get; set; }

    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; } = null!;

    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
}