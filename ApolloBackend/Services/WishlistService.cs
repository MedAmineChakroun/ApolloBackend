using ApolloBackend.Data;
using ApolloBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Services
{
    public interface IWishlistService
    {
        Task<List<WishlistItem>> GetUserWishlist(string userId);
        Task<WishlistItem> AddToWishlist(string userId, string productId, string productName, string productDescription, decimal productPrice);
        Task<bool> RemoveFromWishlist(int wishlistItemId, string userId);
        Task<bool> IsInWishlist(string userId, string productId);
    }

    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;

        public WishlistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WishlistItem>> GetUserWishlist(string userId)
        {
            return await _context.WishlistItems
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.AddedDate)
                .ToListAsync();
        }

        public async Task<WishlistItem> AddToWishlist(string userId, string productId, string productName, string productDescription, decimal productPrice)
        {
            var existingItem = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);

            if (existingItem != null)
            {
                return existingItem;
            }

            var wishlistItem = new WishlistItem
            {
                UserId = userId,
                ProductId = productId,
                ProductName = productName,
                ProductDescription = productDescription,
                ProductPrice = productPrice
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return wishlistItem;
        }

        public async Task<bool> RemoveFromWishlist(int wishlistItemId, string userId)
        {
            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.Id == wishlistItemId && w.UserId == userId);

            if (wishlistItem == null)
            {
                return false;
            }

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsInWishlist(string userId, string productId)
        {
            return await _context.WishlistItems
                .AnyAsync(w => w.UserId == userId && w.ProductId == productId);
        }
    }
} 