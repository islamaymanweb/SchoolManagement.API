using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using Serilog;

namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator, Teacher")]
public class SubjectController(ISubjectService subjectService) : ControllerBase
{
	private readonly ISubjectService _subjectService = subjectService;

	[HttpPost("add")]
	public async Task<IActionResult> AddSubjectWithAssignments([FromBody] SubjectDto dto)
	{
		if (dto == null)
		{
			return BadRequest("Invalid input data.");
		}

		try
		{
			await _subjectService.AddSubjectWithAssignmentsAsync(dto);
			return Ok(new { message = "Subject was added successfully." });
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while adding subject with assignments");
			return Problem(detail: ex.Message, statusCode: 500, title: "Server error");
		}
	}

	[HttpDelete("{id}")]
	[Authorize(Roles = "Administrator")]
	public async Task<IActionResult> DeleteSubject(int id)
	{
		try
		{
			await _subjectService.DeleteSubjectAsync(id);
			return Ok(new { message = "Subject was deleted." });
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while deleting subject");
			return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
		}
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetSubjectById(int id)
	{
		try
		{
			var subject = await _subjectService.GetSubjectByIdAsync(id);
			if (subject == null)
			{
				return NotFound(new { message = "Subject not found." });
			}
			return Ok(subject);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while retrieving subject");
			return Problem(detail: ex.Message, statusCode: 500, title: "Server error");
		}
	}

	[HttpGet("paged")]
	public async Task<IActionResult> GetSubjectsPaged([FromQuery] PagedRequest request)
	{
		try
		{
			var result = await _subjectService.GetSubjectsPaged(request);
			return Ok(result);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while retrieving subjects");
			return Problem(detail: ex.Message, statusCode: 500, title: "Server error");
		}
	}

	[HttpPut("update")]
	public async Task<IActionResult> UpdateSubjectWithAssignments([FromBody] SubjectDto dto)
	{
		if (dto == null)
		{
			return BadRequest("Invalid input data.");
		}

		try
		{
			await _subjectService.UpdateSubjectWithAssignmentsAsync(dto);
			return Ok(new { message = "Subject was updated." });
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while updating subject with assignments");
			return Problem(detail: ex.Message, statusCode: 500, title: "Server error");
		}
	}
}
