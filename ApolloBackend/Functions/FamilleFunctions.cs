using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class FamilleFunctions : IFamille
    {
        private readonly ERPContext _Erpcontext;

        public FamilleFunctions(ERPContext ERPContext)
        {
            _Erpcontext = ERPContext;
        }

        public async Task<List<ListeFamille>> GetFamilles()
        {
            return await _Erpcontext.ListeFamilles.ToListAsync();
        }
        public async Task<int> GetNbFamilles()
        {
            return await _Erpcontext.ListeFamilles.CountAsync();
        }
    }
}
