using ApolloBackend.Data;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        public ClientsController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
      //  [Authorize(Roles ="admin")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            return await _db.Clients.OrderByDescending(r=>r.TiersDateCreate).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return client;
        }
        [HttpGet("code/{tiersCode}")]
        public async Task<ActionResult<Client>> GetByTiersCode(string tiersCode)
        {
            var query = _db.Clients.AsQueryable();
            if (!string.IsNullOrEmpty(tiersCode))
            {
                query = query.Where(c => c.TiersCode == tiersCode);
            }
            return await query.FirstOrDefaultAsync();
        }
        [HttpGet("count")]
        public async Task<ActionResult> GetNbClients()
        {
            int count = await _db.Clients.CountAsync();
            return Ok(count);
        }
        [HttpPut("{id}")]
  
        public async Task<IActionResult> Update(int id, ClientUpdateRequestDto dto)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();

            // Trim whitespace from all string properties
            var trimmedName = dto.Name?.Trim();
            var trimmedAddress = dto.Address?.Trim();
            var trimmedPostalCode = dto.PostalCode?.Trim();
            var trimmedCity = dto.City?.Trim();
            var trimmedCountry = dto.Country?.Trim();
            var trimmedPhone = dto.Phone?.Trim();
         

            // Update client properties with trimmed values
            client.TiersIntitule = trimmedName;
            client.TiersAdresse1 = trimmedAddress;
            client.TiersCodePostal = trimmedPostalCode;
            client.TiersVille = trimmedCity;
            client.TiersPays = trimmedCountry;
            client.TiersTel1 = trimmedPhone;
            client.TiersFlag = dto.Flag;

            // Find and update associated user's UserName if exists
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.ClientId == id);

            if (user != null && user.UserName != trimmedName)
            {
                user.UserName = trimmedName;
                user.NormalizedUserName = trimmedName?.ToUpper();
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPatch("updateFlag/{id}")]
        public async Task<IActionResult> UpdateFlag(int id, [FromQuery] int newFlag)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound($"Client with ID {id} not found.");
            }

            client.TiersFlag = newFlag;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find user first (by ClientId)
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.ClientId == id);

            // Delete user if exists
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            // Then delete client
            var client = await _db.Clients.FindAsync(id);
            if (client != null)
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
            }

            return NoContent();
        }

        [HttpGet("role/{id}")]
        public async Task<IActionResult> getRole(int id)
        {
            var user = await _userManager.Users
        .FirstOrDefaultAsync(u => u.ClientId == id);

            if (user == null)
                return NotFound("User not found");

            // Get roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { UserId = user.Id, Roles = roles });
        }
    }
}
public class ClientUpdateRequestDto
{
    public string Name { get; set; }  // Updates both Client.TiersIntitule and User.UserName
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }

    public int Flag { get; set; }
}