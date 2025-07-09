using SchoolManagement.API.DTOs;

namespace SchoolManagement.API.Interfaces;

public interface ITeacherService
{
    Task<TeacherDto?> GetTeacherByIdAsync(string id);
    Task<IEnumerable<TeacherDto>> GetTeachersAsync();
}