using ApolloBackend.Entities;

namespace ApolloBackend.Interfaces
{
    public interface IProduits
    {
        Task<List<ListeArticle>> GetProduits();
        Task<List<ListeArticle>> GetProduitsByFamille(string famille);
    }
}
