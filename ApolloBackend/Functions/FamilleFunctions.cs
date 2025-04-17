using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class FamilleFunctions : IFamille
    {
        private readonly AppDbContext _context;

        public FamilleFunctions(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Famille>> GetFamilles()
        {
            return await _context.Familles.ToListAsync();
        }
        public async Task<int> GetNbFamilles()
        {
            return await _context.Familles.CountAsync();
        }
    }
}
