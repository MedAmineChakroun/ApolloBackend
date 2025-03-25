using ApolloBackend.Configurations;
using ApolloBackend.Migrations;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthManagementController : ControllerBase
    {
        private readonly ILogger<AuthManagementController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly JwtConfig _jwtConfig;
        public AuthManagementController(ILogger<AuthManagementController> logger,
            UserManager<User> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request Payload");
            // check if emai exist
            var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);
            if (emailExist != null)
                return BadRequest("email exists!");
            var newUser = new User()
            {
                Email = requestDto.Email,
                UserName = requestDto.Name,
            };


            var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
            if (!isCreated.Succeeded)
                return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());

            //set customer role to auth user
            await _userManager.AddToRoleAsync(newUser, "customer");

            //generate token
            return Ok(new RegistrationRequestResponse()
            {
                Result = true,
                Token = await GenerateJwtTokenAsync(newUser)
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDtos requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid request data." });

            var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);
            if (existingUser == null)
                return BadRequest(new { Message = "Invalid email" });

            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);
            if (!isPasswordValid)
                return BadRequest(new { Message = "Invalid  password." });

            var token = await GenerateJwtTokenAsync(existingUser);
            return Ok(new LoginRequestResponse()
            {
                Token = token,
                Result = true
            });
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

            var userRoles = await _userManager.GetRolesAsync(user);

            //set jwt claims
            var claims = new List<Claim>
                {
                  new Claim("Id", user.Id),
                  new Claim("UserName", user.UserName),
                  new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 };

            // Add role claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),

                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}