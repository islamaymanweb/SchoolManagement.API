

using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.DTOs;

public class StudentForAttendanceDto
{
    public int StudentId { get; set; }
    public string FullName { get; set; } = null!;
    public AttendanceStatus Status { get; set; } // null jeśli jeszcze nie zarejestrowano
}
