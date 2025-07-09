using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using Serilog;
using System.Security.Claims;



namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GradeController(
	IGradeService gradeService,
	IStudentService studentService,
	ITeacherService teacherService) : ControllerBase
{
	private readonly IGradeService _gradeService = gradeService;
	private readonly IStudentService _studentService = studentService;
	private readonly ITeacherService _teacherService = teacherService;

	[Authorize(Roles = "Teacher")]
	[HttpPost("add")]
	public async Task<IActionResult> AddGrade([FromBody] GradeCreateDto dto)
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var teacher = await _teacherService.GetTeacherByIdAsync(userId);
			if (teacher == null) return NotFound(new { message = "Teacher not found." });

			var grade = await _gradeService.AddGradeAsync(dto, teacher.Id);
			return Ok(grade);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while adding grade.");
			return BadRequest(new { message = "An error occurred while adding the grade." });
		}
	}

	[Authorize(Roles = "Student")]
	[HttpGet("student/paged")]
	public async Task<IActionResult> GetGradesForStudentPaged([FromQuery] PagedRequest request)
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var student = await _studentService.GetStudentByIdAsync(userId);
			if (student == null) return NotFound(new { message = "Student not found." });

			var grades = await _gradeService.GetGradesForStudentPaged(request, student.Id);
			return Ok(grades);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while fetching grades for student.");
			return BadRequest(new { message = "An error occurred while fetching grades." });
		}
	}

	[Authorize(Roles = "Teacher")]
	[HttpGet("teacher/paged")]
	public async Task<IActionResult> GetGradesForTeacherPaged([FromQuery] PagedRequest request)
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var teacher = await _teacherService.GetTeacherByIdAsync(userId);
			if (teacher == null) return NotFound(new { message = "Teacher not found." });

			var grades = await _gradeService.GetGradesForTeacherPaged(request, teacher.Id);
			return Ok(grades);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while fetching grades for teacher.");
			return BadRequest(new { message = "An error occurred while fetching grades." });
		}
	}

	[HttpGet("students/{subjectId}/{classId}")]
	[Authorize(Roles = "Teacher")]
	public async Task<IActionResult> GetStudentsForSubjectAndClass(int subjectId, int classId)
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var teacher = await _teacherService.GetTeacherByIdAsync(userId);
			if (teacher == null) return NotFound(new { message = "Teacher not found." });

			var students = await _gradeService.GetStudentsForSubjectAndClassAsync(teacher.Id, subjectId, classId);
			return Ok(students);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while fetching students for grading.");
			return BadRequest(new { message = "Failed to retrieve students." });
		}
	}

	[Authorize(Roles = "Teacher")]
	[HttpGet("subjects")]
	public async Task<IActionResult> GetSubjectsForCurrentTeacher()
	{
		try
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId)) return Unauthorized();

			var teacher = await _teacherService.GetTeacherByIdAsync(userId);
			if (teacher == null) return NotFound(new { message = "Teacher not found." });

			var subjects = await _gradeService.GetSubjectsForCurrentTeacherAsync(teacher.Id);
			return Ok(subjects);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error while fetching subjects for teacher.");
			return BadRequest(new { message = "An error occurred while fetching subjects." });
		}
	}
}
