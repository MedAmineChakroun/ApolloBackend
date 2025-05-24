using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface ISynchronisationSage
    {
        public Task<List<DocumentVente>> SynchroniseCommandes();
        public Task<bool> SynchroniseArticles(string codeArt);
        public Task<bool> SynchroniseClients(string codeClient);
        public Task<bool> DeleteArticle(string codeArt);
        public Task<bool> DeleteClient(string codeClient);
        public Task<bool> DeleteCommande(string NumDocument);

    }
}
