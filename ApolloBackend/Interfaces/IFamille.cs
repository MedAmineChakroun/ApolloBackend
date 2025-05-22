using ApolloBackend.Entities;
using ApolloBackend.Models;

namespace ApolloBackend.Interfaces
{
    public interface IFamille
    {
        Task<List<ListeFamille>> GetFamilles();
        Task<int> GetNbFamilles();

        Task<ListeFamille> GetFamilleByIntitule(string id);
   
   
    }
}