using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Services;
public class ScheduleService(AppDbContext context) : IScheduleService
{
	private readonly AppDbContext _context = context;

	public async Task<ScheduleEntryDto> AddEntryAsync(ScheduleEntryDto dto)
	{
		// Validate data presence
		var @class = await _context.Classes.FindAsync(dto.ClassId)
			?? throw new Exception("Class not found.");

		var subject = await _context.Subjects.FindAsync(dto.SubjectId)
			?? throw new Exception("Subject not found.");

		var teacher = await _context.Teachers.FindAsync(dto.TeacherId)
			?? throw new Exception("Teacher not found.");

		// Optionally, check if an entry already exists
		var exists = await _context.Schedules.AnyAsync(s =>
			s.ClassId == dto.ClassId &&
			s.DayOfWeek == dto.DayOfWeek &&
			s.StartTime == TimeSpan.Parse(dto.StartTime));

		if (exists)
			throw new Exception("An entry for this class already exists on this day and time.");

		// Create new entry
		var entry = new Schedule
		{
			ClassId = dto.ClassId,
			SubjectId = dto.SubjectId,
			TeacherId = dto.TeacherId,
			DayOfWeek = dto.DayOfWeek,
			StartTime = TimeSpan.Parse(dto.StartTime)
		};

		await _context.Schedules.AddAsync(entry);
		await _context.SaveChangesAsync();

		return new ScheduleEntryDto
		{
			Id = entry.Id,
			ClassId = entry.ClassId,
			SubjectId = entry.SubjectId,
			TeacherId = entry.TeacherId,
			SubjectName = subject.Name,
			TeacherFullName = $"{teacher.FirstName} {teacher.LastName}",
			DayOfWeek = entry.DayOfWeek,
			StartTime = entry.StartTime.ToString(@"hh\:mm"),
		};
	}

	public async Task<List<ClassWithScheduleDto>> GetClassesWithScheduleAsync()
	{
		var result = await _context.Classes
			.Select(c => new ClassWithScheduleDto
			{
				Id = c.Id,
				Name = c.Name,
				EntryCount = c.Schedules.Count()
			})
			.OrderBy(c => c.Name)
			.ToListAsync();

		return result;
	}

	public async Task<ScheduleForClassDto?> GetScheduleForClassAsync(int classId)
	{
		var classEntity = await _context.Classes
			.Include(c => c.Schedules)
				.ThenInclude(s => s.Subject)
			.Include(c => c.Schedules)
				.ThenInclude(s => s.Teacher)
			.FirstOrDefaultAsync(c => c.Id == classId);

		if (classEntity == null)
			return null;

		var entries = classEntity.Schedules
			.OrderBy(e => e.DayOfWeek)
			.ThenBy(e => e.StartTime)
			.Select(e => new ScheduleEntryDto
			{
				Id = e.Id,
				DayOfWeek = e.DayOfWeek,
				StartTime = e.StartTime.ToString(@"hh\:mm"),
				SubjectName = e.Subject.Name,
				TeacherFullName = $"{e.Teacher.FirstName} {e.Teacher.LastName}"
			})
			.ToList();

		return new ScheduleForClassDto
		{
			ClassId = classEntity.Id,
			ClassName = classEntity.Name,
			Entries = entries
		};
	}

	public async Task<List<StudentScheduleEntryDto>> GetScheduleForStudentAsync(string userId)
	{
		var student = await _context.Students
			.AsNoTracking()
			.Where(s => s.UserId == userId)
			.Select(s => new
			{
				s.Id,
				s.ClassId
			})
			.FirstOrDefaultAsync();

		if (student == null)
		{
			throw new Exception("No student associated with this user account was found.");
		}

		var schedule = await _context.Schedules
			.Where(s => s.ClassId == student.ClassId)
			.Include(s => s.Class)
			.Include(s => s.Subject)
			.Include(s => s.Teacher)
			.OrderBy(s => s.DayOfWeek)
			.ThenBy(s => s.StartTime)
			.Select(s => new StudentScheduleEntryDto
			{
				DayOfWeek = s.DayOfWeek,
				StartTime = s.StartTime.ToString(@"hh\:mm"),
				ClassName = s.Class.Name,
				SubjectName = s.Subject.Name,
				TeacherFullName = $"{s.Teacher.FirstName} {s.Teacher.LastName}"
			})
			.ToListAsync();

		return schedule;
	}

	public async Task<List<TeacherScheduleEntryDto>> GetScheduleForTeacherAsync(string userId)
	{
		var teacher = await _context.Teachers
			.AsNoTracking()
			.FirstOrDefaultAsync(t => t.UserId == userId);

		if (teacher == null)
		{
			throw new Exception("No teacher associated with this user account was found.");
		}

		var schedule = await _context.Schedules
			.Where(s => s.TeacherId == teacher.Id)
			.Include(s => s.Class)
			.Include(s => s.Subject)
			.OrderBy(s => s.DayOfWeek)
			.ThenBy(s => s.StartTime)
			.Select(s => new TeacherScheduleEntryDto
			{
				DayOfWeek = s.DayOfWeek,
				StartTime = s.StartTime.ToString(@"hh\:mm"),
				ClassName = s.Class.Name,
				SubjectName = s.Subject.Name
			})
			.ToListAsync();

		return schedule;
	}

	public async Task<List<SubjectWithTeachersDto>> GetSubjectsForClassAsync(int classId)
	{
		var assignments = await _context.ClassSubjects
			.Where(cs => cs.ClassId == classId)
			.Include(cs => cs.Subject)
			.Include(cs => cs.Teacher)
			.ToListAsync();

		var grouped = assignments
			.GroupBy(cs => new { cs.SubjectId, cs.Subject.Name })
			.Select(g => new SubjectWithTeachersDto
			{
				SubjectId = g.Key.SubjectId,
				SubjectName = g.Key.Name,
				Teachers = g.Select(t => new TeacherDto
				{
					Id = t.Teacher.Id,
					FirstName = t.Teacher.FirstName,
					LastName = t.Teacher.LastName,
				}).DistinctBy(t => t.Id).ToList()
			})
			.ToList();

		return grouped;
	}
}
