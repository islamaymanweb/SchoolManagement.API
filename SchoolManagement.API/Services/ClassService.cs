using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using SchoolManagement.API.Models.Entities;
using Serilog;

namespace SchoolManagement.API.Services;

public class ClassService(AppDbContext context) : IClassService
{
    private readonly AppDbContext _context = context;

    public async Task AddClass(ClassDto classDto)
    {
        var newClass = new Class
        {
            Name = classDto.Name,
            HomeroomTeacherId = classDto.HomeroomTeacherId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
        };

        if (classDto.AssignedStudentIds.Any())
        {
            var students = await _context.Students
                .ToListAsync();

            var users = students.Where(u => classDto.AssignedStudentIds.Contains(u.Id)).ToList();

            if (users.Count == 0)
            {
				throw new Exception("There are no students to add.");

			}

			foreach (var student in users)
            {
                student.Class = newClass;
            }
        }

        await _context.Classes.AddAsync(newClass);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteClass(int id)
    {
        var classToDelete = await _context.Classes
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classToDelete == null)
        {
			throw new Exception("No class found to delete.");

		}

		// Odpinanie uczniów (ustawiamy ClassId = null)
		foreach (var student in classToDelete.Students)
        {
            student.ClassId = null;
        }

        _context.Classes.Remove(classToDelete);
        await _context.SaveChangesAsync();
    }

    public async Task<ClassDto> GetClassById(int id)
    {
        var classEntity = await _context.Classes
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (classEntity == null)
        {
			throw new Exception("The class was not found.");

		}

		return new ClassDto
        {
            Id = classEntity.Id,
            Name = classEntity.Name,
            CreatedAt = classEntity.CreatedAt,
            UpdatedAt = classEntity.UpdatedAt,
            HomeroomTeacherId = classEntity.HomeroomTeacherId,
            AssignedStudentIds = classEntity.Students.Select(s => s.Id).ToList(),
        };
    }

    public async Task<IEnumerable<ClassDto>> GetClassesAsync()
    {
        var classes = await _context.Classes
            .AsNoTracking()
            .Select(c => new ClassDto
            {
                Id = c.Id,
                Name = c.Name,
            })
            .OrderBy(c => c.Name)
            .ToListAsync();

        return classes;
    }

    public async Task<PagedClasses> GetClassesPaged(PagedRequest request)
    {
        var query = _context.Classes
            .AsNoTracking()
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.CreatedAt,
                c.UpdatedAt,
                HomeroomTeacherName = c.HomeroomTeacher != null
                    ? c.HomeroomTeacher.FirstName + " " + c.HomeroomTeacher.LastName
                    : null,
                StudentCount = c.Students.Count()
            });

        if (!string.IsNullOrEmpty(request.SortColumn))
        {
            var isDescending = request.SortDirection?.ToLower() == "desc";
            query = request.SortColumn.ToLower() switch
            {
                "name" => isDescending
                    ? query.OrderByDescending(e => e.Name)
                    : query.OrderBy(e => e.Name),

                "createdat" => isDescending
                    ? query.OrderByDescending(e => e.CreatedAt)
                    : query.OrderBy(e => e.CreatedAt),

                "updatedAt" => isDescending
                    ? query.OrderByDescending(e => e.UpdatedAt)
                    : query.OrderBy(e => e.UpdatedAt),

                _ => throw new ArgumentException($"Invalid sort column: {request.SortColumn}")
            };
        }
        else
        {
            query = query.OrderByDescending(c => c.CreatedAt);
        }

        int totalRecords = await query.CountAsync();

        var classes = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var classesDto = classes.Select(item => new ClassDto
        {
            Id = item.Id,
            Name = item.Name,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt,
            HomeroomTeacherName = item.HomeroomTeacherName,
            StudentCount = item.StudentCount,
        }).ToList();

        return new PagedClasses
        {
            TotalRecords = totalRecords,
            Data = classesDto
        };
    }

    public async Task UpdateClass(ClassDto classDto)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var classToUpdate = await _context.Classes
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == classDto.Id);

            if (classToUpdate == null)
            {
				Log.Error($"Class with ID {classDto.Id} was not found.");
				throw new Exception($"Class with ID {classDto.Id} was not found.");

			}

			classToUpdate.Name = classDto.Name;
            classToUpdate.HomeroomTeacherId = classDto.HomeroomTeacherId;
            classToUpdate.UpdatedAt = DateTime.Now;

            var currentStudentIds = classToUpdate.Students.Select(s => s.Id).ToList();
            var targetStudentIds = classDto.AssignedStudentIds;

            var studentsToRemove = await _context.Students
                .Where(s => currentStudentIds.Contains(s.Id) && !targetStudentIds.Contains(s.Id))
                .ToListAsync();

            foreach (var student in studentsToRemove)
            {
                student.ClassId = null;
            }

            var studentsToAdd = await _context.Students
                .Where(s => targetStudentIds.Contains(s.Id) && s.ClassId != classDto.Id)
                .ToListAsync();

            foreach (var student in studentsToAdd)
            {
                student.ClassId = classDto.Id;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
			Log.Error(ex, "An error occurred while updating the class");
			throw new Exception("An error occurred while updating the class", ex);

		}
	}
}