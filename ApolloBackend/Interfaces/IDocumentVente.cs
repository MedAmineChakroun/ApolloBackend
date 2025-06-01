using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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
        Task<DocumentVente> UpdateDocumentEtat(int id, int etat , string note);
        Task<DocumentVente> UpdateDocumentFlag(int id, int flag);

        Task<bool> DeleteCommande(int id);
        Task<bool> UpdateDocumentVente(int id, DocumentVenteDto dto);
        Task<bool> isProductPurshased(string tiersCode, string artCode);

        Task<bool> HasOrders(string tiersCode);
        Task<bool> HasOrdersForArticles(string artCode);
    }
}
