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
            DocumentVente.DocTht = documentVenteDto.DocTht;
            DocumentVente.DocTtc = documentVenteDto.DocTtc;
            DocumentVente.DocEtat = 0;
            DocumentVente.DocFlag = 0;
            _context.DocumentVentes.Add(DocumentVente);
            await _context.SaveChangesAsync();

          

            return DocumentVente;
        }
        public async Task<DocumentVente> GetDocumentByPieceCode(string PieceCode)
        {
            var query = _context.DocumentVentes.AsQueryable();
            if (!string.IsNullOrEmpty(PieceCode))
            {
                query = query.Where(p => p.DocPiece == PieceCode);
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<int> GetNbCommande()
        {
            return await _context.DocumentVentes.CountAsync();
        }

        public async Task<int> GetNbCommandeAddedLastweek()
        {
            // 1. Compute the cutoff datetime (7 days ago from now)
            var oneWeekAgo = DateTime.Now.AddDays(-7);

            // 2. Filter and count
            return await _context.DocumentVentes
                                 .Where(d => d.DocDate >= oneWeekAgo)
                                 .CountAsync();
        }
        public async Task<bool> DeleteCommande(int id)
        {
            var doc = await _context.DocumentVentes.FirstOrDefaultAsync(d => d.DocId == id);
            if (doc == null)
                return false;

            _context.DocumentVentes.Remove(doc);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<DocumentVente> UpdateDocumentEtat(int id, int etat, string note)
        {
            var doc = await _context.DocumentVentes.FirstOrDefaultAsync(d => d.DocId == id);
            if (doc == null)
                return null;
            doc.DocEtat = etat;
            doc.DocNote = note;
            doc.DocFlag = 0;
            _context.DocumentVentes.Update(doc);
            await _context.SaveChangesAsync();
            return doc;

        }
        public async Task<DocumentVente> UpdateDocumentFlag(int id, int flag)
        {
            var doc = await _context.DocumentVentes.FirstOrDefaultAsync(d => d.DocId == id);
            if (doc == null)
                return doc ;
            doc.DocFlag = flag;
          
            _context.DocumentVentes.Update(doc);
            await _context.SaveChangesAsync();
            return doc;
        }
        public async Task<bool> UpdateDocumentVente(int id, DocumentVenteDto dto)
        {
            var document = await _context.DocumentVentes.FindAsync(id);
            if (document == null)
                return false;

            // Mise à jour des champs depuis le DTO
            document.DocTiersCode = dto.DocTiersCode;
            document.DocTiersIntitule = dto.DocTiersIntitule;
            document.DocTht = dto.DocTht;
            document.DocTtc = dto.DocTtc;
            document.DocFlag = 0;
            _context.DocumentVentes.Update(document);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> isProductPurshased(string tiersCode, string artCode)
        {
            // Check if both parameters are provided
            if (string.IsNullOrEmpty(tiersCode) || string.IsNullOrEmpty(artCode))
                return false;


            var exists = await _context.DocumentVentes
                .Where(doc => doc.DocTiersCode == tiersCode && doc.DocEtat == 1)
                .Join(_context.DocumentVenteLignes,
                      doc => doc.DocPiece,
                      ligne => ligne.LigneDocPiece,
                      (doc, ligne) => ligne)
                .AnyAsync(ligne => ligne.LigneArtCode == artCode);

            return exists;
        }
        public async Task<bool> HasOrders(string tiersCode)
        {
            // Check if TiersCode is provided
            if (string.IsNullOrEmpty(tiersCode))
                return false;

            // Check if there's at least one order for this TiersCode
            return await _context.DocumentVentes
                .AnyAsync(doc => doc.DocTiersCode == tiersCode);
        }
        public async Task<bool> HasOrdersForArticles(string artCode)
        {
            // Check if ArtCode is provided
            if (string.IsNullOrEmpty(artCode))
                return false;
            // Check if there's at least one order for this ArtCode
            return await _context.DocumentVenteLignes
                .AnyAsync(ligne => ligne.LigneArtCode == artCode);
        }

    }

}
