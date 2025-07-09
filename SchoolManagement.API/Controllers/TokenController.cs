using System.Security.Claims;
using backend.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Models.Entities;

namespace SchoolManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TokenController : ControllerBase
	{
		private readonly ITokenService _tokenService;
		private readonly UserManager<User> _userManager;

		public TokenController(ITokenService tokenService, UserManager<User> userManager)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		[Authorize]
		[HttpGet("refresh")]
		public async Task<IActionResult> RefreshToken()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				return BadRequest("Missing user ID in the request.");
			}

			var user = await _userManager.FindByIdAsync(userId);
			if (user == null)
			{
				return NotFound("User not found.");
			}

			var token = _tokenService.GenerateJwtTokenForUser(user);

			return Ok(new { Token = token });
		}
	}
}
