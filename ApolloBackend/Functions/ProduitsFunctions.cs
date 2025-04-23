using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace ApolloBackend.Functions
{
    public class ProduitsFunctions : IProduits
    {
        private readonly AppDbContext _context;

        public ProduitsFunctions(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Article>> GetProduits()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetProduitById(int id)
        {
            var produit = await _context.Articles.AsQueryable()
                .Where(p => p.ArtId == id)
                .FirstOrDefaultAsync();
            if (produit == null)
            {
                return null;
            }
            return produit;

        }
        public async Task<List<Article>> GetProduitsByFamille(string famille = null)
        {
            var query = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(famille))
            {
                query = query.Where(p => p.ArtFamille == famille);
            }

            return await query.ToListAsync();
        }
        public async Task<int> GetNbProduits()
        {
            return await _context.Articles.CountAsync();
        }
        public async Task<Article> GetProduitByCode(string code)
        {
            var produit = await _context.Articles.AsQueryable()
                .Where(p => p.ArtCode == code)
                .FirstOrDefaultAsync();
            if (produit == null)
            {
                return null;
            }
            return produit;
        }
        public async Task<List<Article>> GetTopSellingProducts(int limit = 25)
        {
           

            // First, get the top selling article codes with their quantities
            var topSellingCodes = await _context.DocumentVenteLignes
                .GroupBy(ligne => ligne.LigneArtCode)
                .Select(g => new
                {
                    ArticleCode = g.Key,
                    TotalQuantity = g.Sum(x => x.LigneQte ?? 0)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(limit)
                .ToListAsync();

            // Then fetch the full article details for these codes
            var articleCodes = topSellingCodes.Select(x => x.ArticleCode).ToList();

            var articles = await _context.Articles
                .Where(a => articleCodes.Contains(a.ArtCode))
                .ToListAsync();

            // Sort the articles in the same order as the topSellingCodes
            return articles
                .OrderBy(a => articleCodes.IndexOf(a.ArtCode))
                .ToList();
        }
        public async Task<List<ProductWithRatingDto>> GetTopRatedProductsWithRatings(int limit = 25)
        {

            // Get products with their average rating
            var topRatedProducts = await _context.Ratings
                .GroupBy(r => r.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    AverageRating = g.Average(r => r.Stars),
                    RatingCount = g.Count()
                })
                .OrderByDescending(x => x.AverageRating)
                .ThenByDescending(x => x.RatingCount)
                .Take(limit)
                .Join(_context.Articles,
                    rating => rating.ProductId,
                    article => article.ArtId,
                    (rating, article) => new ProductWithRatingDto
                    {
                        Article = article,
                        AverageRating = rating.AverageRating,
                        RatingCount = rating.RatingCount
                    })
                .ToListAsync();

            return topRatedProducts;
        }
    }

}
