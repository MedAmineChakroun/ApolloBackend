using ApolloBackend.Functions;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduitById(int id)
        {
            var produit = await _produitsFunctions.GetProduitById(id);

            if (produit == null)
            {
            }

            return Ok(produit);
        }
        [HttpGet("byFamille")]
        public async Task<IActionResult> GetByFamille([FromQuery] string famille)
        {
            var produits = await _produitsFunctions.GetProduitsByFamille(famille);
            return Ok(produits);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await _produitsFunctions.GetNbProduits();
            return Ok(count);
        }
        [HttpGet("Code/{code}")]
        public async Task<IActionResult> GetProduitByCode([FromRoute] string code)
        {
            var produit = await _produitsFunctions.GetProduitByCode(code);
            if (produit == null)
            {
                return NotFound($"Product with code {code} not found");
            }
            return Ok(produit);
        }

    }
}
