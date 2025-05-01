using ApolloBackend.Configurations;
using ApolloBackend.Data;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
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
        private readonly AppDbContext _dbContext;

        public AuthManagementController(
            ILogger<AuthManagementController> logger,
            UserManager<User> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            AppDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
            _dbContext = dbContext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] ClientRegistrationRequestDto requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Request Payload");

            // Check if email exists
            var emailExist = await _userManager.FindByEmailAsync(requestDto.Email);
            if (emailExist != null)
                return BadRequest("Email already exists!");

            // Create associated Client record FIRST
            var client = new Client
            {
                TiersIntitule = requestDto.Name,
                TiersAdresse1 = requestDto.Address,
                TiersCodePostal = requestDto.PostalCode,
                TiersVille = requestDto.City,
                TiersPays = requestDto.Country,
                TiersTel1 = requestDto.Phone,
                TiersFlag = 0,
                TiersDateCreate = DateTime.Now,
                TiersEmail = requestDto.Email,
            };

            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync(); // This generates the ClientId

            // Now create the User with the ClientId
            var newUser = new User()
            {
                Email = requestDto.Email,
                UserName = requestDto.Name,
                PhoneNumber = requestDto.Phone,
                ClientId = client.TiersId
            };

            // Create user in Identity system
            var isCreated = await _userManager.CreateAsync(newUser, requestDto.Password);
            if (!isCreated.Succeeded)
            {
                // Rollback client creation if user creation fails
                _dbContext.Clients.Remove(client);
                await _dbContext.SaveChangesAsync();
                return BadRequest(isCreated.Errors.Select(x => x.Description).ToList());
            }

            
                // Set customer role to auth user
                await _userManager.AddToRoleAsync(newUser, "customer");

            // Generate token
            return Ok(new RegistrationRequestResponse()
            {
                Result = true,
                Token = await GenerateJwtTokenAsync(newUser)
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] ClientLoginRequestDtos requestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid request data." });

            var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);
            if (existingUser == null)
                return BadRequest(new { Message = "Invalid email" });

            var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);
            if (!isPasswordValid)
                return BadRequest(new { Message = "Invalid password." });

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

            // Set jwt claims
            var claims = new List<Claim>
            {
                new Claim("ClientId", user.ClientId.ToString()),
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
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            return jwtToken;
        }
    }
}