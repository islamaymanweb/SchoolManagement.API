using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Interfaces.Services;
using Serilog;


namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendanceController : ControllerBase
{
	private readonly IAttendanceService _attendanceService;
	private readonly ITeacherService _teacherService;

	public AttendanceController(IAttendanceService attendanceService, ITeacherService teacherService)
	{
		_attendanceService = attendanceService;
		_teacherService = teacherService;
	}

	[HttpGet("students/{scheduleId}")]
	[Authorize(Roles = "Teacher")]
	public async Task<IActionResult> GetStudentsForSchedule(int scheduleId)
	{
		try
		{
			var result = await _attendanceService.GetStudentsForScheduleAsync(scheduleId);
			return Ok(result);
		}
		catch (Exception ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpGet("today-lessons")]
	[Authorize(Roles = "Teacher")]
	public async Task<IActionResult> GetTodayLessons()
	{
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

		if (string.IsNullOrEmpty(userId))
			return Unauthorized();

		var teacher = await _teacherService.GetTeacherByIdAsync(userId);
		if (teacher == null)
			return NotFound(new { message = "Teacher not found." });

		var result = await _attendanceService.GetTodayLessonsForTeacherAsync(teacher.Id);
		return Ok(result);
	}

	[HttpPost("save/{scheduleId}")]
	[Authorize(Roles = "Teacher")]
	public async Task<IActionResult> SaveAttendance(int scheduleId, [FromBody] List<AttendanceCreateDto> attendanceList)
	{
		try
		{
			await _attendanceService.SaveAttendanceAsync(scheduleId, attendanceList);
			return Ok(new { message = "Attendance has been saved successfully." });
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error occurred while saving attendance.");
			return BadRequest(new { message = "An error occurred while saving attendance." });
		}
	}
}
