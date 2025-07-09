using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.DTOs;

public class AttendanceCreateDto
{
    public int StudentId { get; set; }
    public AttendanceStatus Status { get; set; }
}