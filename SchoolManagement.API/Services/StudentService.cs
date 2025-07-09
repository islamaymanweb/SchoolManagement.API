using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;

namespace SchoolManagement.API.Services;

public class StudentService(AppDbContext context) : IStudentService
{
    private readonly AppDbContext _context = context;

    public async Task<StudentDto?> GetStudentByIdAsync(string id)
    {
        var student = await _context.Students
            .AsNoTracking()
            .Where(t => t.UserId == id)
            .Select(t => new StudentDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
            })
            .FirstOrDefaultAsync();

        return student;
    }

    public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
    {
        var students = await _context.Students
            .AsNoTracking()
            .Include(student => student.Class)
            .Select(student => new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassName = student.Class!.Name,
            })
            .OrderBy(student => student.LastName)
            .ThenBy(student => student.FirstName)
            .ToListAsync();

        return students;
    }
}