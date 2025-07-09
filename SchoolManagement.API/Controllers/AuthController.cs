using SchoolManagement.API.DTOs;
using SchoolManagement.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using backend.Interfaces.Services;
namespace SchoolManagement.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		private readonly SignInManager<User> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly UserManager<User> _userManager;

		public AuthController(
			SignInManager<User> signInManager,
			ITokenService tokenService,
			UserManager<User> userManager)
		{
			_signInManager = signInManager;
			_tokenService = tokenService;
			_userManager = userManager;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] Login userLoginData)
		{
			if (!ModelState.IsValid || userLoginData.UserName == null || userLoginData.Password == null)
			{
				return BadRequest(ModelState);
			}

			var signInResult = await _signInManager.PasswordSignInAsync(userLoginData.UserName, userLoginData.Password, isPersistent: true, lockoutOnFailure: true);

			var user = await _userManager.FindByNameAsync(userLoginData.UserName);

			if (user == null)
			{
				var message = "User with the provided identifier was not found.";
				return NotFound(new { message });
			}

			if (!user.IsActive)
			{
				var message = "Account is locked. Please contact the administrator.";
				return Unauthorized(new { message });
			}

			if (!signInResult.Succeeded)
			{
				var message = "Invalid username or password.";
				return Unauthorized(new { message });
			}

			var token = _tokenService.GenerateJwtTokenForUser(user);
			return Ok(new { Token = token });
		}

		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok();
		}
	}
}
