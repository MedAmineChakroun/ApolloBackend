using ApolloBackend.Models;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet("user/{tiersId}")]
        public async Task<ActionResult<List<Article>>> GetUserWishlist(int tiersId)
        {
            try
            {
                // Récupérer les WishlistItem de l'utilisateur
                var wishlistItems = await _wishlistService.GetUserWishlist(tiersId);

                // Vérifier si la liste est vide ou nulle
                if (wishlistItems == null || !wishlistItems.Any())
                {
                    return NotFound("Aucun article dans la wishlist pour cet utilisateur.");
                }

                // Extraire les articles de la wishlist
                var articles = wishlistItems.Select(w => w.Article).ToList();

                // Retourner les articles sous forme de liste
                return Ok(articles);
            }
            catch (Exception ex)
            {
                // Gérer toute exception inattendue
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }


        // POST: api/Wishlist/add
        [HttpPost("add")]
        public async Task<ActionResult<WishlistItem>> AddToWishlist([FromQuery] int tiersId, [FromQuery] int artId)
        {
            var item = await _wishlistService.AddToWishlist(tiersId, artId);
            return Ok(item);
        }

        // DELETE: api/Wishlist/remove
        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveFromWishlist([FromQuery] int tiersId, [FromQuery] int artId)
        {
            var result = await _wishlistService.RemoveFromWishlist(tiersId, artId);
            if (!result)
                return NotFound("L'élément n'existe pas ou n'appartient pas à cet utilisateur.");

            return NoContent();
        }

        // GET: api/Wishlist/exists
        [HttpGet("exists")]
        public async Task<ActionResult<bool>> IsInWishlist([FromQuery] int tiersId, [FromQuery] int artId)
        {
            var exists = await _wishlistService.IsInWishlist(tiersId, artId);
            return Ok(exists);
        }
    }
}
