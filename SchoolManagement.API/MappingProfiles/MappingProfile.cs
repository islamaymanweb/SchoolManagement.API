using AutoMapper;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models.Entities;

namespace backend.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Student, StudentDto>().ReverseMap();
        CreateMap<Teacher, TeacherDto>().ReverseMap();
        CreateMap<Class, ClassDto>().ReverseMap();
        CreateMap<Subject, SubjectDto>().ReverseMap();
        CreateMap<Schedule, ScheduleDto>().ReverseMap();
        CreateMap<Attendance, AttendanceDto>().ReverseMap();
    }
}