using ApolloBackend.Entities;

namespace ApolloBackend.Interfaces
{
    public interface IFamille
    {
        Task<List<ListeFamille>> GetFamilles();
    }
}