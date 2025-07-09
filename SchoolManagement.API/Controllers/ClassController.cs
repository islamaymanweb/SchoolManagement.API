using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using Serilog;

namespace SchoolManagement.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClassController(IClassService classService) : ControllerBase
{
	private readonly IClassService _classService = classService;

	[Authorize(Roles = "Administrator")]
	[HttpPost("add")]
	public async Task<IActionResult> AddClass([FromBody] ClassDto classDto)
	{
		try
		{
			await _classService.AddClass(classDto);
			return Ok(new { message = "The class has been added successfully." });
		}
		catch (Exception e)
		{
			var message = $"An error occurred while adding the class: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Administrator")]
	[HttpDelete("delete/{id}")]
	public async Task<IActionResult> DeleteClass(int id)
	{
		try
		{
			await _classService.DeleteClass(id);
			return Ok(new { message = "The class has been deleted successfully." });
		}
		catch (Exception e)
		{
			var message = $"An error occurred while deleting the class: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Administrator, Teacher")]
	[HttpGet("get/{id}")]
	public async Task<IActionResult> GetClassById(int id)
	{
		try
		{
			var classDto = await _classService.GetClassById(id);
			if (classDto == null)
			{
				return NotFound(new { message = "No class was found with the provided ID." });
			}
			return Ok(classDto);
		}
		catch (Exception e)
		{
			var message = $"An error occurred while retrieving the class: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Administrator, Teacher")]
	[HttpGet("all")]
	public async Task<IActionResult> GetClasses()
	{
		try
		{
			var classes = await _classService.GetClassesAsync();
			return Ok(classes);
		}
		catch (Exception e)
		{
			var message = $"An error occurred while retrieving classes: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Administrator")]
	[HttpGet("paged")]
	public async Task<IActionResult> GetClassesPaged([FromQuery] PagedRequest pagedRequest)
	{
		try
		{
			var classes = await _classService.GetClassesPaged(pagedRequest);
			return Ok(classes);
		}
		catch (Exception e)
		{
			var message = $"An error occurred while retrieving paginated classes: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}

	[Authorize(Roles = "Administrator")]
	[HttpPut("update")]
	public async Task<IActionResult> UpdateClass([FromBody] ClassDto classDto)
	{
		try
		{
			await _classService.UpdateClass(classDto);
			return Ok(new { message = "The class has been updated successfully." });
		}
		catch (Exception e)
		{
			var message = $"An error occurred while updating the class: {e.Message}";
			Log.Error(message);
			return BadRequest(new { message });
		}
	}
}
