using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class FamilleFunctions : IFamille
    {
        private readonly ERPContext _context;

        public FamilleFunctions(ERPContext context)
        {
            _context = context;
        }

        public async Task<List<ListeFamille>> GetFamilles()
        {
            return await _context.ListeFamilles.ToListAsync();
        }
    }
}
