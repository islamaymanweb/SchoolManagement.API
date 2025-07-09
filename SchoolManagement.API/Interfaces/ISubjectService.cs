using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces;

public interface ISubjectService
{
    Task AddSubjectWithAssignmentsAsync(SubjectDto dto);
    Task DeleteSubjectAsync(int subjectId);
    Task<SubjectDto> GetSubjectByIdAsync(int id);
    Task<PagedSubjects> GetSubjectsPaged(PagedRequest request);
    Task UpdateSubjectWithAssignmentsAsync(SubjectDto dto);
}