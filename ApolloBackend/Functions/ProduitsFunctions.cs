using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class ProduitsFunctions : IProduits
    {
        private readonly AppDbContext _context;

        public ProduitsFunctions(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Article>> GetProduits()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetProduitById(int id)
        {
            var produit = await _context.Articles.AsQueryable()
                .Where(p => p.ArtId == id)
                .FirstOrDefaultAsync();
            if (produit == null)
            {
                return null;
            }
            return produit;

        }
        public async Task<List<Article>> GetProduitsByFamille(string famille = null)
        {
            var query = _context.Articles.AsQueryable();

            if (!string.IsNullOrEmpty(famille))
            {
                query = query.Where(p => p.ArtFamille == famille);
            }

            return await query.ToListAsync();
        }
        public async Task<int> GetNbProduits()
        {
            return await _context.Articles.CountAsync();
        }
        public async Task<Article> GetProduitByCode(string code)
        {
            var produit = await _context.Articles.AsQueryable()
                .Where(p => p.ArtCode == code)
                .FirstOrDefaultAsync();
            if (produit == null)
            {
                return null;
            }
            return produit;
        }
    }
}
