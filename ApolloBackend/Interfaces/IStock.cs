using ApolloBackend.Entities;

namespace ApolloBackend.Interfaces
{
    public interface IStock 
    {
         Task<List<ListeStock>> GetArticleStocksAsync();
        Task<ListeStock?> GetStockByRefAsync(string arRef);

    }
}
