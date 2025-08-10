using Application.DTO.Auth;
using Application.DTO.Users;
using Application.Services_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TCCD_Task.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;   

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("auth/signup")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _userService.RegisterUserAsync(request);
            SetTokenCookie(result.Token);
            _logger.LogInformation("User registered successfully with email: {Email}", request.Email);
            return Ok(result);
        }

        [HttpPost("auth/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _userService.LoginUserAsync(request);
            SetTokenCookie(result.Token);
            _logger.LogInformation("User logged in successfully with email: {Email}", request.Email);
            return Ok(result);
        }

        [HttpPost("auth/logout")]
        [Authorize]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            _logger.LogInformation("User logged out successfully");
            return Ok("Logged out successfully");
        }


        [HttpGet("users")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? throw new UnauthorizedAccessException());
            var result = await _userService.GetUserDetailsAsync(userId);
            if (result == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", userId);
                return NotFound("User not found");
            }
            _logger.LogInformation("User details retrieved for ID: {UserId}", userId);
            return Ok(result);
        }

        [HttpPatch("users")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            var userId = Guid.Parse(User.FindFirstValue("id") ?? throw new UnauthorizedAccessException());
            var response=await _userService.UpdateUserAsync(userId, request);
            if (response == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update", userId);
                return NotFound("User not found");
            }
            _logger.LogInformation("User with ID {UserId} updated successfully", userId);
            return Ok(response);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("jwtToken", token, cookieOptions);
        }
    }
}
