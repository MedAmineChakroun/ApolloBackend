using ApolloBackend.Data;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class DocumentVenteLigneFunctions : IDocumentVenteLigne
    {
        private readonly AppDbContext _context;
        public DocumentVenteLigneFunctions(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<DocumentVenteLigne>> getByDocumentVenteLignePiece(string docPiece)
        {
            var lignes = await _context.DocumentVenteLignes
                .Where(l => l.LigneDocPiece == docPiece)
                .ToListAsync();
            return lignes;
        }
        public async Task<DocumentVenteLigne> createDocumentVenteLigne(DocumentVenteLigneDto documentVenteligneDto)
        {
            var ligne = new DocumentVenteLigne
            {
                LigneDocPiece = documentVenteligneDto.LigneDocPiece,
                LigneArtCode = documentVenteligneDto.LigneArtCode,
                LigneArtDesi = documentVenteligneDto.LigneArtDesi,
                LigneQte = documentVenteligneDto.LigneQte,
                LignePu = documentVenteligneDto.LignePu,
                LigneHt = documentVenteligneDto.LigneHt,
                LigneTtc = documentVenteligneDto.LigneTtc
            };

            await _context.DocumentVenteLignes.AddAsync(ligne);
            await _context.SaveChangesAsync();
            return ligne;
        }
    }

}
