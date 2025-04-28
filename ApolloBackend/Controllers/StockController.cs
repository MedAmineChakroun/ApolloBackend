using ApolloBackend.Entities;
using ApolloBackend.Functions;
using ApolloBackend.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStock _stockFunctions;
        public StockController(IStock stockFunctions)
        {
            _stockFunctions = stockFunctions;
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {
            var stocks = await _stockFunctions.GetArticleStocksAsync();
            return Ok(stocks);
        }
        [HttpGet("{arRef}")]
        public async Task<IActionResult> GetStockByRef(string arRef)
        {
            var stock = await _stockFunctions.GetStockByRefAsync(arRef);
            if (stock == null)
            {
                return NotFound(); // 404 if not found
            }
            return Ok(stock); // 200 OK with the stock
        }


    }
}
