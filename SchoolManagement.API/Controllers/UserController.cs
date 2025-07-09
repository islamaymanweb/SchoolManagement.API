using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.DTOs;
using SchoolManagement.API.Interfaces;
using SchoolManagement.API.Models;
using Serilog;
namespace SchoolManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[Authorize(Roles = "Administrator")]
		[HttpPost("add")]
		public async Task<IActionResult> AddNewUser([FromBody] AddUserModel addUserModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (addUserModel.Email == null)
			{
				return BadRequest("Missing username in the request.");
			}

			var result = await _userService.AddNewUserAsync(addUserModel);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return Ok();
		}

		[Authorize(Roles = "Administrator")]
		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			if (id == null)
			{
				return BadRequest(new { message = "Missing user ID in the request." });
			}

			try
			{
				var result = await _userService.DeleteUserAsync(id);
				if (!result.Succeeded)
				{
					return BadRequest(result.Errors);
				}

				return Ok();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred while deleting the user.");
				return StatusCode(500, new { message = "An error occurred while deleting the user. " + ex.Message });
			}
		}

		[HttpGet("logged-user")]
		public async Task<IActionResult> GetLoggedUser()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (userId == null)
			{
				return BadRequest("Missing user ID in the request.");
			}

			var user = await _userService.GetUserByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			return Ok(user);
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet("get-user/{id}")]
		public async Task<IActionResult> GetUserByIdAsync(string id)
		{
			if (id == null)
			{
				return BadRequest(new { message = "Missing user ID in the request." });
			}

			try
			{
				var user = await _userService.GetUserByIdAsync(id);
				if (user == null)
				{
					return NotFound(new { message = "User not found." });
				}

				return Ok(user);
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred while fetching the user.");
				return StatusCode(500, new { message = "An error occurred while fetching the user. " + ex.Message });
			}
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet("paged")]
		public async Task<IActionResult> GetUsersPaged([FromQuery] PagedRequest pagedRequest)
		{
			try
			{
				var users = await _userService.GetUsersPaged(pagedRequest);
				return Ok(users);
			}
			catch (Exception e)
			{
				var message = $"An error occurred while retrieving users: {e.Message}";
				Log.Error(message);
				return BadRequest(new { message });
			}
		}

		[Authorize(Roles = "Administrator")]
		[HttpGet("roles")]
		public async Task<IActionResult> GetRoles()
		{
			try
			{
				var roles = await _userService.GeRolesAsync();
				return Ok(roles);
			}
			catch (Exception e)
			{
				var message = $"An error occurred while retrieving roles: {e.Message}";
				Log.Error(message);
				return BadRequest(new { message });
			}
		}

		[Authorize(Roles = "Administrator")]
		[HttpPut("update")]
		public async Task<IActionResult> UpdateUser([FromBody] UpdateUser updateUser)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				var updateResult = await _userService.UpdateUserAsync(updateUser);
				if (!updateResult)
				{
					return BadRequest("Failed to update the user.");
				}

				return Ok();
			}
			catch (Exception ex)
			{
				Log.Error(ex, "An error occurred while updating the user.");
				return StatusCode(500, "An error occurred while updating the user.");
			}
		}
	}
}
