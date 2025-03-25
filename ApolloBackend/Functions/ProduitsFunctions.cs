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
    }
}
