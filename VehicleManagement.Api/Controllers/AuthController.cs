using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleManagement.Api.Services.Interfaces;

namespace VehicleManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("[POST] /api/auth/login attempt for user {Username}", request.Username);
            try
            {
                var user = await _authService.AuthenticateAsync(request.Username, request.Password);
                if (user == null)
                {
                    _logger.LogWarning("[POST] /api/auth/login failed for user {Username}: invalid credentials", request.Username);
                    return Unauthorized(new ErrorResponse("Invalid credentials"));
                }
                var token = _authService.GenerateJwtToken(user);
                _logger.LogInformation("[POST] /api/auth/login success for user {Username}", request.Username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[POST] /api/auth/login exception for user {Username}: {Message}", request.Username, ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao autenticar usuário."));
            }
        }
    }

}
