using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Interfaces
{
    public interface IDocumentVenteLigne
    {
        Task<List<DocumentVenteLigne>> getDocumentVenteLignes();
        Task<List<DocumentVenteLigne>> getByDocumentVenteLignePiece(string docPiece);

        Task<DocumentVenteLigne> createDocumentVenteLigne(DocumentVenteLigneDto documentVenteligneDto);
        Task<int> GetLignesNb();
        Task<List<DocumentVenteLigne>> getTopDocumentVenteLignes();
    }

}
