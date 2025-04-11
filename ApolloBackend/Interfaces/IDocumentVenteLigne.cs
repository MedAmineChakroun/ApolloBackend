using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;

namespace ApolloBackend.Interfaces
{
    public interface IDocumentVenteLigne
    {
        Task<List<DocumentVenteLigne>> getByDocumentVenteLignePiece(string docPiece);

        Task<DocumentVenteLigne> createDocumentVenteLigne(DocumentVenteLigneDto documentVenteligneDto);
    }
}
