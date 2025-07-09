using SchoolManagement.API.DTOs;

namespace SchoolManagement.API.Interfaces.Services;

public interface IAttendanceService
{
    Task<List<StudentForAttendanceDto>> GetStudentsForScheduleAsync(int scheduleId);
    Task<List<LessonForAttendanceDto>> GetTodayLessonsForTeacherAsync(int teacherId);
    Task SaveAttendanceAsync(int scheduleId, List<AttendanceCreateDto> attendanceList);
}
