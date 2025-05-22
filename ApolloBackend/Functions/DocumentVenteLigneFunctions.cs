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

        public async Task<bool> UpdateDocumentVenteLigne(int id, DocumentVenteLigneDto dto)
        {
            var ligne = await _context.DocumentVenteLignes.FindAsync(id);
            if (ligne == null)
                return false;

            ligne.LigneQte = dto.LigneQte;
            ligne.LignePu = dto.LignePu;
            ligne.LigneHt = dto.LigneHt;
            ligne.LigneTtc = dto.LigneTtc;
            ligne.LigneArtCode = dto.LigneArtCode;
            ligne.LigneArtDesi = dto.LigneArtDesi;
            ligne.LigneDocPiece = dto.LigneDocPiece;

            _context.DocumentVenteLignes.Update(ligne);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> deleteDocumentVenteLigne(int id)
        {
            var ligne = await _context.DocumentVenteLignes.FindAsync(id);
            if (ligne == null)
                return false;

            _context.DocumentVenteLignes.Remove(ligne);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
