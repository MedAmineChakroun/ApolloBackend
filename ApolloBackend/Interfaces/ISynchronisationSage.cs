using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface ISynchronisationSage
    {
        public Task<List<DocumentVente>> SynchroniseCommandes();
        public Task<bool> SynchroniseArticles(string codeArt);
        public Task<bool> SynchroniseClients(string codeClient);
    }
}
