using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class ClientsFunctions : IClients
    {
        private readonly AppDbContext _context;

        public ClientsFunctions(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Client?> GetClientByCodeAsync(string code)
        {
            return await _context.Clients
                           .Where(f => f.TiersCode == code)
                           .FirstOrDefaultAsync();

        }

        public async Task<List<Client>> GetClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }
        public async Task<bool> UpdateFlag(int id, int newFlag)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return false;
            }

            client.TiersFlag = newFlag;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}