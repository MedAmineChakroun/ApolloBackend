using ApolloBackend.Entities;
using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface IProduits
    {
        Task<List<Article>> GetProduits();
        Task<Article> GetProduitById(int id);
        Task<List<Article>> GetProduitsByFamille(string famille);
    }
}
