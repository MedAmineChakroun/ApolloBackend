using ApolloBackend.Models;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<ActionResult<List<WishlistItem>>> GetUserWishlist()
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var wishlist = await _wishlistService.GetUserWishlist(userId);
            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<ActionResult<WishlistItem>> AddToWishlist([FromBody] WishlistItemDto wishlistItemDto)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var wishlistItem = await _wishlistService.AddToWishlist(
                userId,
                wishlistItemDto.ProductId,
                wishlistItemDto.ProductName,
                wishlistItemDto.ProductDescription,
                wishlistItemDto.ProductPrice
            );

            return Ok(wishlistItem);
        }

        [HttpDelete("{wishlistItemId}")]
        public async Task<ActionResult> RemoveFromWishlist(int wishlistItemId)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _wishlistService.RemoveFromWishlist(wishlistItemId, userId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("check/{productId}")]
        public async Task<ActionResult<bool>> IsInWishlist(string productId)
        {
            var userId = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var isInWishlist = await _wishlistService.IsInWishlist(userId, productId);
            return Ok(isInWishlist);
        }
    }

    public class WishlistItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
    }
} 