using ApolloBackend.Data;
using ApolloBackend.Entities;
using ApolloBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Functions
{
    public class StockFunctions : IStock
    {
        private readonly ERPContext _Erpcontext;

        public StockFunctions(ERPContext ERPContext)
        {
            _Erpcontext = ERPContext;
        }

        public async Task<List<ListeStock>> GetArticleStocksAsync()
        {
            var stocks = await _Erpcontext.ListeStocks
                .Select(s => new ListeStock
                {
                    ArRef = s.ArRef,
                    AsQteSto = s.AsQteSto
                })
                .ToListAsync();

            return stocks;
        }
        public async Task<ListeStock?> GetStockByRefAsync(string arRef)
        {
            return await _Erpcontext.ListeStocks
                .Where(s => s.ArRef == arRef)
                .Select(s => new ListeStock
                {
                    ArRef = s.ArRef,
                    AsQteSto = s.AsQteSto
                })
                .FirstOrDefaultAsync();
        }

    }
}
