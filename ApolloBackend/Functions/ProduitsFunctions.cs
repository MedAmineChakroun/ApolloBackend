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
            return await _context.Articles.FirstOrDefaultAsync(p => p.ArtId == id);
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
            return await _context.Articles.FirstOrDefaultAsync(p => p.ArtCode == code);
        }

        // 🔽 CREATE
        public async Task<Article> CreateProduit(Article produit)
        {
            _context.Articles.Add(produit);
            await _context.SaveChangesAsync();
            return produit;
        }

        // 🔽 UPDATE
        public async Task<Article?> UpdateProduit(Article produit)
        {
            var existing = await _context.Articles.FindAsync(produit.ArtId);
            if (existing == null) return null;

            // Update fields
            existing.ArtCode = produit.ArtCode;
            existing.ArtIntitule = produit.ArtIntitule;
            existing.ArtFamille = produit.ArtFamille;
            existing.ArtPrixVente = produit.ArtPrixVente;
            existing.ArtPrixAchat = produit.ArtPrixAchat;
            existing.ArtEtat = produit.ArtEtat;
            existing.ArtUnite = produit.ArtUnite;
            existing.ArtImageUrl = produit.ArtImageUrl;
            existing.ArtTvaTaux = produit.ArtTvaTaux;

            await _context.SaveChangesAsync();
            return existing;
        }

        // 🔽 DELETE
        public async Task<bool> DeleteProduit(int id)
        {
            var produit = await _context.Articles.FindAsync(id);
            if (produit == null) return false;

            _context.Articles.Remove(produit);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
