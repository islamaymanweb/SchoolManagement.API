using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interfaces;
using Serilog;

namespace SchoolManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrator,Teacher")]
public class StudentController(IStudentService studentService) : ControllerBase
{
	private readonly IStudentService _studentService = studentService;

	[HttpGet("all")]
	public async Task<IActionResult> GetStudents()
	{
		try
		{
			var students = await _studentService.GetStudentsAsync();
			return Ok(students);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Error occurred while fetching students: {Message}, {StackTrace}", ex.Message, ex.StackTrace);
			return StatusCode(500, new
			{
				message = "An error occurred while fetching students.",
				error = ex.Message
			});
		}
	}
}
