using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StackExchange.Redis;
using System.Text.Json;

namespace ApolloBackend.Functions
{
    public class ProduitsFunctions : IProduits
    {
        private readonly AppDbContext _context;
        private readonly IDatabase _redis;
        public ProduitsFunctions(AppDbContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis.GetDatabase();
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
        public async Task<List<Article>> GetSimilarProductByCategory(string famIntitule, int limit)
        {
            var produits = await _context.Articles
                .Where(p => p.ArtFamille == famIntitule)
                .Take(limit) // Apply the limit
                .ToListAsync();

            return produits;
        }
        public async Task<Article> CreateProduit(ArticleDto articleDto)
        {
            var article = new Article
            {
                ArtIntitule = articleDto.ArtIntitule,
                ArtFamille = articleDto.ArtFamille,
                ArtPrixVente = articleDto.ArtPrixVente,
                ArtPrixAchat = articleDto.ArtPrixAchat,
                ArtImageUrl = articleDto.ArtImageUrl,
                ArtTvaTaux = articleDto.ArtTvaTaux,
                ArtUnite = articleDto.ArtUnite,
                ArtFlag = 0,
                ArtEtat = 0,
                ArtDateCreate = DateTime.Now
            };
            
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> UpdateProduit(Article article)
        {
            var existingArticle = await _context.Articles.FindAsync(article.ArtId);
            if (existingArticle == null)
                return null;

            existingArticle.ArtIntitule = article.ArtIntitule;
            existingArticle.ArtFamille = article.ArtFamille;
            existingArticle.ArtPrixVente = article.ArtPrixVente;
            existingArticle.ArtPrixAchat = article.ArtPrixAchat;
            existingArticle.ArtEtat = article.ArtEtat;
            existingArticle.ArtUnite = article.ArtUnite;
            existingArticle.ArtImageUrl = article.ArtImageUrl;
            existingArticle.ArtTvaTaux = article.ArtTvaTaux;
            existingArticle.ArtFlag = article.ArtFlag;
            // Don't touch ArtCode or ArtDateCreate

            _context.Articles.Update(existingArticle);
            await _context.SaveChangesAsync();
            return existingArticle;
        }

        public async Task<bool> DeleteProduit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
                return false;

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Article>> GetRecommendations(string tiersCode, int limit)
        {
            string cacheKey = $"recommendations:{tiersCode}:{limit}";

            // Try to get from Redis cache first
            var cached = await _redis.StringGetAsync(cacheKey);
            if (!cached.IsNullOrEmpty)
            {
                var cachedArticles = JsonSerializer.Deserialize<List<Article>>(cached);
                return cachedArticles;
            }

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://localhost:5000/recommend?user_id={tiersCode}&n={limit}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get recommendations from Python model");

            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonDocument.Parse(jsonString);
            var itemIds = jsonObject.RootElement
                                    .GetProperty("recommendations")
                                    .EnumerateArray()
                                    .Select(x => x.GetProperty("item_id").GetString())
                                    .ToList();

            var recommendedArticles = new List<Article>();

            foreach (var code in itemIds)
            {
                string articleCacheKey = $"article:{code}";

                // Try to get article from Redis
                var cachedArticle = await _redis.StringGetAsync(articleCacheKey);
                if (!cachedArticle.IsNullOrEmpty)
                {
                    var article = JsonSerializer.Deserialize<Article>(cachedArticle);
                    recommendedArticles.Add(article);
                }
                else
                {
                    var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArtCode == code);
                    if (article != null)
                    {
                        recommendedArticles.Add(article);
                        await _redis.StringSetAsync(articleCacheKey, JsonSerializer.Serialize(article), TimeSpan.FromHours(1));
                    }
                }
            }

            // Maintain original recommendation order
            recommendedArticles = recommendedArticles
                .OrderBy(a => itemIds.IndexOf(a.ArtCode))
                .ToList();

            // Cache the entire list for 30 minutes
            await _redis.StringSetAsync(cacheKey, JsonSerializer.Serialize(recommendedArticles), TimeSpan.FromMinutes(30));

            return recommendedArticles;
        }
        public async Task<List<Article>> GetProduitsByCodesLimited(List<string> codes, int count)
        {
            string cacheKey = $"cart-recommendations:{string.Join("-", codes)}:{count}";
            // Try Redis cache
            var cached = await _redis.StringGetAsync(cacheKey);
            if (!cached.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Article>>(cached);
            }

            // Prepare request to Python model
            var requestBody = new
            {
                item_ids = codes,
                count = count
            };
            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsync("http://localhost:5001/api/recommend/cart", content);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get cart recommendations from Python model");

            var jsonString = await response.Content.ReadAsStringAsync();
            var jsonObject = JsonDocument.Parse(jsonString);

            // Extract item IDs directly from the "recommendations" array which contains strings
            var itemIds = jsonObject.RootElement
                                .GetProperty("recommendations")
                                .EnumerateArray()
                                .Select(x => x.GetString())
                                .Where(id => id != null)
                                .ToList();

            var recommendedArticles = new List<Article>();
            foreach (var code in itemIds)
            {
                string articleCacheKey = $"article:{code}";
                var cachedArticle = await _redis.StringGetAsync(articleCacheKey);
                if (!cachedArticle.IsNullOrEmpty)
                {
                    recommendedArticles.Add(JsonSerializer.Deserialize<Article>(cachedArticle));
                }
                else
                {
                    var article = await _context.Articles.FirstOrDefaultAsync(a => a.ArtCode == code);
                    if (article != null)
                    {
                        recommendedArticles.Add(article);
                        await _redis.StringSetAsync(articleCacheKey, JsonSerializer.Serialize(article), TimeSpan.FromHours(1));
                    }
                }
            }

            // Maintain the order from recommendations
            recommendedArticles = recommendedArticles
                .OrderBy(a => itemIds.IndexOf(a.ArtCode))
                .ToList();

            // Cache full result
            await _redis.StringSetAsync(cacheKey, JsonSerializer.Serialize(recommendedArticles), TimeSpan.FromMinutes(30));

            return recommendedArticles;
        }

        public async Task<bool> UpdateProduitFlag(int id, int newFlag)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return false;
            }

            article.ArtFlag = newFlag;
            await _context.SaveChangesAsync();

            return true;
        }

    }

}
