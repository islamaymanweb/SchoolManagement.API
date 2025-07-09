using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Interfaces;

public interface IClassService
{
    Task AddClass(ClassDto classDto);
    Task DeleteClass(int id);
    Task<ClassDto> GetClassById(int id);
    Task<IEnumerable<ClassDto>> GetClassesAsync();
    Task<PagedClasses> GetClassesPaged(PagedRequest request);
    Task UpdateClass(ClassDto classDto);
}