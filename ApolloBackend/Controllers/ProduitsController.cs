using ApolloBackend.Functions;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitsFunctions _produitsFunctions;

        public ProduitsController(ProduitsFunctions produitsFunctions)
        {
            _produitsFunctions = produitsFunctions;
        }

        [HttpGet]
        public async Task<JsonResponseData> GetListeProduit()
        {
            return new JsonResponseData
            {
                Data = new
                {
                   Produits = await _produitsFunctions.GetProduits(),
                },
                IsSuccess = true,
                Message = "" 
            };
        }
        [HttpGet("byFamille")]
        public async Task<IActionResult> GetByFamille([FromQuery] string famille)
        {
            var produits = await _produitsFunctions.GetProduitsByFamille(famille);
            return Ok(produits);
        }
    }
}
