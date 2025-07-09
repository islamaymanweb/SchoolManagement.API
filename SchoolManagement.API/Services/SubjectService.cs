using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using SchoolManagement.API.Models.Entities;
using Serilog;

namespace SchoolManagement.API.Services;
public class SubjectService(AppDbContext context) : ISubjectService
{
	private readonly AppDbContext _context = context;

	public async Task AddSubjectWithAssignmentsAsync(SubjectDto dto)
	{
		await using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var subject = new Subject
			{
				Name = dto.Name,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			await _context.Subjects.AddAsync(subject);
			await _context.SaveChangesAsync(); // required to obtain subject.Id

			var classSubjects = dto.Assignments.Select(a => new ClassSubject
			{
				ClassId = a.ClassId,
				TeacherId = a.TeacherId,
				SubjectId = subject.Id
			}).ToList();

			await _context.ClassSubjects.AddRangeAsync(classSubjects);
			await _context.SaveChangesAsync();

			await transaction.CommitAsync();
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			Log.Error(ex, "Error while adding subject with assignments");
			throw new Exception("Error while adding subject with assignments", ex);
		}
	}

	public async Task DeleteSubjectAsync(int subjectId)
	{
		await using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var subject = await _context.Subjects
				.Include(s => s.ClassSubjects)
				.FirstOrDefaultAsync(s => s.Id == subjectId);

			if (subject == null)
			{
				throw new Exception($"Subject with ID {subjectId} not found.");
			}

			// Remove relationships from ClassSubjects table
			if (subject.ClassSubjects.Any())
			{
				_context.ClassSubjects.RemoveRange(subject.ClassSubjects);
			}

			// Remove the subject itself
			_context.Subjects.Remove(subject);

			await _context.SaveChangesAsync();
			await transaction.CommitAsync();
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			Log.Error(ex, $"Error while deleting subject ID {subjectId}");
			throw new Exception("An error occurred while deleting the subject.", ex);
		}
	}

	public async Task<SubjectDto> GetSubjectByIdAsync(int id)
	{
		var subject = await _context.Subjects
			.AsNoTracking()
			.Where(s => s.Id == id)
			.Select(s => new SubjectDto
			{
				Id = s.Id,
				Name = s.Name,
				Assignments = s.ClassSubjects.Select(cs => new SubjectAssignmentDto
				{
					ClassId = cs.ClassId,
					TeacherId = cs.TeacherId
				}).ToList()
			})
			.FirstOrDefaultAsync();

		if (subject == null)
			throw new Exception("Subject not found.");

		return subject;
	}

	public async Task<PagedSubjects> GetSubjectsPaged(PagedRequest request)
	{
		var query = _context.Subjects
			.AsNoTracking()
			.Select(s => new
			{
				s.Id,
				s.Name,
				s.CreatedAt,
				s.UpdatedAt,
				Assignments = s.ClassSubjects.Select(cs => new
				{
					ClassName = cs.Class.Name,
					TeacherFullName = cs.Teacher.FirstName + " " + cs.Teacher.LastName
				}).ToList()
			});

		// Sorting
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

				"updatedat" => isDescending
					? query.OrderByDescending(e => e.UpdatedAt)
					: query.OrderBy(e => e.UpdatedAt),

				_ => throw new ArgumentException($"Invalid sort column: {request.SortColumn}")
			};
		}
		else
		{
			query = query.OrderByDescending(s => s.CreatedAt);
		}

		var totalRecords = await query.CountAsync();

		var subjects = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync();

		var subjectDtos = subjects.Select(item => new SubjectDto
		{
			Id = item.Id,
			Name = item.Name,
			CreatedAt = item.CreatedAt,
			UpdatedAt = item.UpdatedAt,
			AssignmentsDto = item.Assignments.Select(a => $"{a.ClassName} ({a.TeacherFullName})").ToList()
		}).ToList();

		return new PagedSubjects
		{
			TotalRecords = totalRecords,
			Data = subjectDtos
		};
	}

	public async Task UpdateSubjectWithAssignmentsAsync(SubjectDto dto)
	{
		await using var transaction = await _context.Database.BeginTransactionAsync();

		try
		{
			var subject = await _context.Subjects
				.FirstOrDefaultAsync(s => s.Id == dto.Id);

			if (subject == null)
			{
				throw new Exception($"Subject with ID {dto.Id} not found.");
			}

			// 1. Update basic data
			subject.Name = dto.Name;
			subject.UpdatedAt = DateTime.Now;
			_context.Subjects.Update(subject);

			// 2. Remove existing assignments
			var existingAssignments = await _context.ClassSubjects
				.Where(cs => cs.SubjectId == dto.Id)
				.ToListAsync();

			_context.ClassSubjects.RemoveRange(existingAssignments);

			// 3. Add new assignments
			var newAssignments = dto.Assignments.Select(a => new ClassSubject
			{
				ClassId = a.ClassId,
				TeacherId = a.TeacherId,
				SubjectId = dto.Id
			}).ToList();

			await _context.ClassSubjects.AddRangeAsync(newAssignments);

			// 4. Save changes
			await _context.SaveChangesAsync();
			await transaction.CommitAsync();
		}
		catch (Exception ex)
		{
			await transaction.RollbackAsync();
			Log.Error(ex, "Error while editing subject with assignments");
			throw new Exception("Error while editing subject", ex);
		}
	}
}
