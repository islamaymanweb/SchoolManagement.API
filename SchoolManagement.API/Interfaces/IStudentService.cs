using SchoolManagement.API.DTOs;

namespace SchoolManagement.API.Interfaces;

public interface IStudentService
{
    Task<StudentDto?> GetStudentByIdAsync(string id);
    Task<IEnumerable<StudentDto>> GetStudentsAsync();
}