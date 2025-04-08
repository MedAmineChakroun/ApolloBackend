using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class ProduitsFunctions : IProduits
    {
        private readonly ERPContext _context;

        public ProduitsFunctions(ERPContext context)
        {
            _context = context;
        }
        public async Task<List<ListeArticle>> GetProduits()
        {
          return  await _context.ListeArticles.ToListAsync();
        }

        public async Task<ListeArticle> GetProduitById(int id)
        {
            var produit = await _context.ListeArticles.AsQueryable()
                .Where(p => p.ArtId == id)
                .FirstOrDefaultAsync();
            if (produit == null)
            {
                return null;
            }
            return produit;

        }
        public async Task<List<ListeArticle>> GetProduitsByFamille(string famille = null)
        {
            var query = _context.ListeArticles.AsQueryable();

            if (!string.IsNullOrEmpty(famille))
            {
                query = query.Where(p => p.ArtFamille == famille);
            }

            return await query.ToListAsync();
        }

    }
}
