

using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.DTOs;

public class AttendanceDto
{
    public int StudentId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
}
