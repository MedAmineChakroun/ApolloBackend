using ApolloBackend.Entities;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Interfaces
{
    public interface IProduits
    {
        Task<List<Article>> GetProduits();
        Task<Article> GetProduitById(int id);
        Task<List<Article>> GetProduitsByFamille(string famille);
        Task<Article> GetProduitByCode(string code);
        Task<int> GetNbProduits();
        Task<List<Article>> GetTopSellingProducts(int limit);
        Task<List<ProductWithRatingDto>> GetTopRatedProductsWithRatings(int limit);

        Task<List<Article>> GetSimilarProductByCategory(string famIntitule,int limit);
        Task<Article> CreateProduit(ArticleDto articleDto);
        Task<Article> UpdateProduit(Article article);
        Task<bool> DeleteProduit(int id);
    }
}
