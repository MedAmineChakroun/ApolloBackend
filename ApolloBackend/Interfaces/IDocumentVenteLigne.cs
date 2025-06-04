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
        Task<bool> deleteDocumentVenteLigne(int id);
        Task<bool> UpdateDocumentVenteLigne(int id, DocumentVenteLigneDto dto);
        Task<List<object>> GetTop10BestSellingProducts();
        Task<List<object>> GetTop4BestSellingCategories();

    }

}
