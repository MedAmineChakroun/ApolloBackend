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
        public async Task<List<DocumentVenteLigne>> getDocumentVenteLignes()
        {
            return await _context.DocumentVenteLignes.ToListAsync();
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
        public async Task<int> GetLignesNb()
        {
            return await _context.DocumentVenteLignes.CountAsync();
        }
        public async Task<List<DocumentVenteLigne>> getTopDocumentVenteLignes()
        {
            var lignes = await _context.DocumentVenteLignes
                .OrderByDescending(l => l.LigneDocPiece) // or use a date field if available
                .ToListAsync();

            // Distinct by LigneArtCode, keeping the most recent one per article
            var topDistinctLignes = lignes
                .GroupBy(l => l.LigneArtCode)
                .Select(g => g.First())
                .Take(10)
                .ToList();

            return topDistinctLignes;
        }


    }

}
