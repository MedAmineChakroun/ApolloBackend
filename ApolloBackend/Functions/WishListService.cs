using ApolloBackend.Data;
using ApolloBackend.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Services
{
    public interface IWishlistService
    {
        Task<List<WishlistItem>> GetUserWishlist(int TiersId);
        Task<WishlistItem> AddToWishlist(int TiersId, int ArtId);
        Task<bool> RemoveFromWishlist(int TiersId, int wishlistItemId);
        Task<bool> IsInWishlist(int TiersId, int ArtId);
    }
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _context;

        public WishlistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WishlistItem>> GetUserWishlist(int TiersId)
        {
            return await _context.WishlistItems
                .Include(w => w.Article)
                .Include(w => w.Client)
                .Where(w => w.TiersId == TiersId)
                .OrderByDescending(w => w.AddedDate)
                .ToListAsync();
        }

        public async Task<WishlistItem> AddToWishlist(int TiersId, int ArtId)
        {
            // Vérifier si l'article est déjà dans la wishlist du client
            var existingItem = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.TiersId == TiersId && w.ArtId == ArtId);

            if (existingItem != null)
            {
                return existingItem;
            }

            // Récupérer le client et l'article de la base de données
            var client = await _context.Clients.FindAsync(TiersId);
            var article = await _context.Articles.FindAsync(ArtId);

            if (client == null || article == null)
            {
                throw new Exception("Client ou Article introuvable.");
            }

            var wishlistItem = new WishlistItem
            {
                TiersId = TiersId,
                ArtId = ArtId,
                Client = client,
                Article = article,
            };

            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();

            return wishlistItem;
        }

        public async Task<bool> RemoveFromWishlist(int TiersId, int artId)
        {
            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(w => w.ArtId == artId && w.TiersId == TiersId);

            if (wishlistItem == null)
                return false;

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsInWishlist(int TiersId, int ArtId)
        {
            return await _context.WishlistItems
                .AnyAsync(w => w.TiersId == TiersId && w.ArtId == ArtId);
        }
    }
}
