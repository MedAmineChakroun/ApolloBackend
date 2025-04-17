using ApolloBackend.Entities;
using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface IFamille
    {
        Task<List<Famille>> GetFamilles();
        Task<int> GetNbFamilles();
    }
}