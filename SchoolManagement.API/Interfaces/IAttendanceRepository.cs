using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Interfaces.Repositories;

public interface IAttendanceRepository
{
    Task AddAsync(Attendance attendance);
    Task<List<Attendance>> GetByStudentIdAsync(int studentId);
}
