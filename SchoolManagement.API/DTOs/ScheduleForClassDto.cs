namespace SchoolManagement.API.DTOs;

public class ScheduleForClassDto
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = null!;
    public List<ScheduleEntryDto> Entries { get; set; } = new();
}