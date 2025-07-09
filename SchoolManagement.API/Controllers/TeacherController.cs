using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using Serilog;
namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator")]
public class TeacherController : ControllerBase
{
	private readonly ITeacherService _teacherService;

	public TeacherController(ITeacherService teacherService)
	{
		_teacherService = teacherService;
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetTeachers()
	{
		try
		{
			var teachers = await _teacherService.GetTeachersAsync();
			return Ok(teachers);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error occurred while fetching teachers: {Message}, {StackTrace}", ex.Message, ex.StackTrace);
			return StatusCode(500, new
			{
				message = "An error occurred while retrieving the teachers.",
				error = ex.Message
			});
		}
	}
}
