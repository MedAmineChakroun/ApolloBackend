using ApolloBackend.Data;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class DocumentVenteFunctions : IDocumentVente
    {
        private readonly AppDbContext _context;
        public DocumentVenteFunctions(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<DocumentVente>> GetDocumentVentes()
        {
            return await _context.DocumentVentes.ToListAsync();
        }
        public async Task<List<DocumentVente>> GetDocumentVentesByTiers(string tiersCode)
        {
          var query = _context.DocumentVentes.AsQueryable();
            if (!string.IsNullOrEmpty(tiersCode))
            {
                query = query.Where(p => p.DocTiersCode == tiersCode);
            }
            return await query.ToListAsync();
        }
        public async Task<DocumentVente> CreateDocumentVente(DocumentVenteDto documentVenteDto)
        {
            var DocumentVente = new DocumentVente();
            DocumentVente.DocType = 0;
            DocumentVente.DocTiersCode = documentVenteDto.DocTiersCode;
            DocumentVente.DocTiersIntitule = documentVenteDto.DocTiersIntitule;
            DocumentVente.DocDate = DateTime.Now;

            _context.DocumentVentes.Add(DocumentVente);
            await _context.SaveChangesAsync();
            return DocumentVente;
        }
        public async Task<DocumentVente?> UpdateDocumentVenteTotals(string docPiece, decimal tht, decimal ttc)
        {
            // Find the document by DocPiece
            var document = await _context.DocumentVentes
                .FirstOrDefaultAsync(d => d.DocPiece == docPiece);

            if (document == null)
            {
                // Handle not found case
                return null;
            }

            // Update the totals
            document.DocTht = tht;
            document.DocTtc = ttc;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return document;
        }


    }

}
