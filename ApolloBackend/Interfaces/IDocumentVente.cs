using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Interfaces
{
    public interface IDocumentVente
    {
        Task<List<DocumentVente>> GetDocumentVentes();

        Task<List<DocumentVente>> GetDocumentVentesByTiers(string tiersCode);

        Task<DocumentVente> CreateDocumentVente(DocumentVenteDto documentVenteDto);

        Task<DocumentVente> UpdateDocumentVenteTotals(string idDoc , decimal tht , decimal ttc);

    }
}
