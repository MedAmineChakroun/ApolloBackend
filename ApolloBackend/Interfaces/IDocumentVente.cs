using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Interfaces
{
    public interface IDocumentVente
    {
        Task<List<DocumentVente>> GetDocumentVentes();

        Task<List<DocumentVente>> GetDocumentVentesByTiers(string tiersCode);

        Task<DocumentVente> CreateDocumentVente(DocumentVenteDto documentVenteDto);
        Task<DocumentVente> GetDocumentByPieceCode(string PieceCode);
        Task<int> GetNbCommande();
        Task<int> GetNbCommandeAddedLastweek();
    }
}
