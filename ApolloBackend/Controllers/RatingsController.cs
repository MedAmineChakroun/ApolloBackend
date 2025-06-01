using Microsoft.AspNetCore.Http;
using ApolloBackend.Models;
using ApolloBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RatingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddRating(RatingCreateDto dto)
        {
            // Vérifie que le produit existe avant d'ajouter une note
            var productExists = await _context.Articles.AnyAsync(a => a.ArtId == dto.ProductId);
            if (!productExists)
            {
                return BadRequest("Produit introuvable.");
            }

            Rating rating;

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.ProductId == dto.ProductId && r.UserId == dto.UserId);

            if (existingRating != null)
            {
                // Mise à jour de la note existante
                existingRating.Stars = dto.Stars;
                existingRating.CreatedAt = DateTime.Now;
                rating = existingRating;
            }
            else
            {
                // Création d'une nouvelle note
                rating = new Rating
                {
                    ProductId = dto.ProductId,
                    UserId = dto.UserId,
                    Stars = dto.Stars,
                    CreatedAt = DateTime.Now
                };
                _context.Ratings.Add(rating);
            }

            await _context.SaveChangesAsync();
            _ = Task.Run(async () =>
            {
                try
                {
                    using var httpClient = new HttpClient();
                    var response = await httpClient.PostAsync("http://localhost:5000/light-refresh", null);
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[FLASK] Failed to call light-refresh: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[FLASK] Error calling light-refresh: {ex.Message}");
                }
            });
            return Ok(rating);
        }

        [HttpGet("{productId}/{userId}")]
        public async Task<IActionResult> GetRatingsForProduct(int productId, int userId)
        {
            var rating = await _context.Ratings
                .Include(r => r.client)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);

            if (rating == null)
                return NotFound(new { message = "Rating not found for the specified product and user." });

            return Ok(rating);
        }


        [HttpGet("average/{productId}")]
        public async Task<IActionResult> GetAverageRating(int productId)
        {
            var avg = await _context.Ratings
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (double?)r.Stars) ?? 0;

            return Ok(avg);
        }
        [HttpGet("count/{productId}")]
        public async Task<int> GetRatingCount(int productId)
        {
            return await _context.Ratings
                .Where(r => r.ProductId == productId)
                .CountAsync();
        }
    }
}