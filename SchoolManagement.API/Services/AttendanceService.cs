using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Services;

public class AttendanceService : IAttendanceService
{
    private readonly AppDbContext _context;

    public AttendanceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentForAttendanceDto>> GetStudentsForScheduleAsync(int scheduleId)
    {
        // Znajdź harmonogram z klasą
        var schedule = await _context.Schedules
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null)
			throw new Exception("Lesson plan with the given ID was not found.");


		var today = DateTime.Today;

        // Pobierz uczniów przypisanych do klasy
        var students = await _context.Students
            .Where(s => s.ClassId == schedule.ClassId)
            .Select(s => new StudentForAttendanceDto
            {
                StudentId = s.Id,
                FullName = s.FirstName + " " + s.LastName,
                // Pobierz status obecności jeśli istnieje
                Status = _context.Attendances
                    .Where(a => a.ScheduleId == scheduleId && a.StudentId == s.Id && a.Date.Date == today)
                    .Select(a => a.Status)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return students;
    }

    public async Task<List<LessonForAttendanceDto>> GetTodayLessonsForTeacherAsync(int teacherId)
    {
        var today = DateTime.Now.DayOfWeek;

        var lessons = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Class)
            .Where(s => s.TeacherId == teacherId && s.DayOfWeek == today)
            .OrderBy(s => s.StartTime)
            .Select(s => new LessonForAttendanceDto
            {
                ScheduleId = s.Id,
                SubjectName = s.Subject.Name,
                ClassName = s.Class.Name,
                StartTime = s.StartTime.ToString(@"hh\:mm"),
                HasAttendance = _context.Attendances
                    .Any(a => a.ScheduleId == s.Id && a.Date.Date == DateTime.Today)
            })
            .ToListAsync();

        return lessons;
    }

    public async Task SaveAttendanceAsync(int scheduleId, List<AttendanceCreateDto> attendanceList)
    {
        if (!attendanceList.Any())
			throw new ArgumentException("The attendance list is empty.");


		var today = DateOnly.FromDateTime(DateTime.Today);

        var existingAttendances = await _context.Attendances
            .Where(a => a.ScheduleId == scheduleId && DateOnly.FromDateTime(a.Date) == today)
            .ToListAsync();

        if (existingAttendances.Any())
        {
            _context.Attendances.RemoveRange(existingAttendances);
            await _context.SaveChangesAsync();
        }

        var newAttendances = attendanceList.Select(a => new Attendance
        {
            ScheduleId = scheduleId,
            StudentId = a.StudentId,
            Date = DateTime.Now,
            Status = a.Status
        }).ToList();

        await _context.Attendances.AddRangeAsync(newAttendances);
        await _context.SaveChangesAsync();
    }

}