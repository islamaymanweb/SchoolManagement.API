using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;

namespace SchoolManagement.API.Services;

public class TeacherService(AppDbContext context) : ITeacherService
{
    private readonly AppDbContext _context = context;

    public async Task<TeacherDto?> GetTeacherByIdAsync(string id)
    {
        var teacher = await _context.Teachers
            .AsNoTracking()
            .Where(t => t.UserId == id)
            .Select(t => new TeacherDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
            })
            .FirstOrDefaultAsync();

        return teacher;
    }

    public async Task<IEnumerable<TeacherDto>> GetTeachersAsync()
    {
        var teachers = await _context.Teachers
            .AsNoTracking()
            .Select(teacher => new TeacherDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
            })
            .ToListAsync();

        return teachers;
    }
}