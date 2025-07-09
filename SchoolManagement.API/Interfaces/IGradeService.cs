using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces;

public interface IGradeService
{
    Task<GradeDto> AddGradeAsync(GradeCreateDto dto, int teacherId);
    Task<PagedGrades> GetGradesForStudentPaged(PagedRequest request, int studentId);
    Task<PagedGrades> GetGradesForTeacherPaged(PagedRequest request, int teacherId);
    Task<List<StudentDto>> GetStudentsForSubjectAndClassAsync(int teacherId, int subjectId, int classId);
    Task<List<SubjectWithClassDto>> GetSubjectsForCurrentTeacherAsync(int teacherId);
}