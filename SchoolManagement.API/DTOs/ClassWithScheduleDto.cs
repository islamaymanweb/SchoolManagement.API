namespace SchoolManagement.API.DTOs;

public class ClassWithScheduleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool HasSchedule => EntryCount > 0;
    public int EntryCount { get; set; } // Liczba wpisów w planie lekcji
}
