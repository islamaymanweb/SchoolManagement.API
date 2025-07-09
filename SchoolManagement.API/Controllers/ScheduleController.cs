using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using Serilog;

namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScheduleController(IScheduleService scheduleService) : ControllerBase
{
	private readonly IScheduleService _scheduleService = scheduleService;

	[Authorize(Roles = "Teacher,Admin")]
	[HttpPost("add-entry")]
	public async Task<IActionResult> AddEntry([FromBody] ScheduleEntryDto dto)
	{
		try
		{
			var entry = await _scheduleService.AddEntryAsync(dto);
			return Ok(entry);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while adding the schedule entry: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Teacher,Admin")]
	[HttpGet("classes-with-schedule")]
	public async Task<IActionResult> GetClassesWithSchedule()
	{
		try
		{
			var classes = await _scheduleService.GetClassesWithScheduleAsync();
			return Ok(classes);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while retrieving classes with schedules: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Teacher,Admin,Student")]
	[HttpGet("class/{classId}")]
	public async Task<IActionResult> GetScheduleForClass(int classId)
	{
		try
		{
			var schedule = await _scheduleService.GetScheduleForClassAsync(classId);
			if (schedule == null)
				return NotFound(new { message = "Schedule not found for this class." });

			return Ok(schedule);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while retrieving schedule for class ID {classId}: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Student")]
	[HttpGet("student")]
	public async Task<IActionResult> GetScheduleForStudent()
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
				return Unauthorized();

			var schedule = await _scheduleService.GetScheduleForStudentAsync(userId);
			return Ok(schedule);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while retrieving schedule for the student: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Admin,Teacher,Student")]
	[HttpGet("teacher/{userId}")]
	public async Task<IActionResult> GetScheduleForTeacher(string userId)
	{
		try
		{
			var schedule = await _scheduleService.GetScheduleForTeacherAsync(userId);
			return Ok(schedule);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while retrieving schedule for teacher ID {userId}: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Admin,Teacher,Student")]
	[HttpGet("class/{classId}/subjects")]
	public async Task<IActionResult> GetSubjectsForClass(int classId)
	{
		try
		{
			var subjects = await _scheduleService.GetSubjectsForClassAsync(classId);
			return Ok(subjects);
		}
		catch (Exception ex)
		{
			var message = $"An error occurred while retrieving subjects for class ID {classId}: {ex.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}
}
