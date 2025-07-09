using SchoolManagement.API.DTOs;

namespace SchoolManagement.API.Interfaces;

public interface IScheduleService
{
    Task<ScheduleEntryDto> AddEntryAsync(ScheduleEntryDto dto);
    Task<List<ClassWithScheduleDto>> GetClassesWithScheduleAsync();
    Task<ScheduleForClassDto?> GetScheduleForClassAsync(int classId);
    Task<List<StudentScheduleEntryDto>> GetScheduleForStudentAsync(string userId);
    Task<List<TeacherScheduleEntryDto>> GetScheduleForTeacherAsync(string userId);
    Task<List<SubjectWithTeachersDto>> GetSubjectsForClassAsync(int classId);
}