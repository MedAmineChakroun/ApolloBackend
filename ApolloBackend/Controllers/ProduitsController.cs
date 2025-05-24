using ApolloBackend.Functions;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using ApolloBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

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
                return NotFound($"Product with ID {id} not found");
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
        [HttpGet("topsales/{limit}")]
        public async Task<IActionResult> GetTopSales([FromRoute] int limit )
        {
            var topProducts = await _produitsFunctions.GetTopSellingProducts(limit);
            if (topProducts == null || !topProducts.Any())
            {
                return NotFound("No sales data found");
            }
            return Ok(topProducts);
        }
        [HttpGet("toprated/{limit}")]
        public async Task<IActionResult> GetTopRated([FromRoute] int limit )
        {
            var topRatedProducts = await _produitsFunctions.GetTopRatedProductsWithRatings(limit);
            if (topRatedProducts == null || !topRatedProducts.Any())
            {
                return NotFound("No rated products found");
            }

            return Ok(new
            {
                Products = topRatedProducts,
                Count = topRatedProducts.Count
            });
        }
        [HttpGet("similar/{category}/{limit}")]
        public async Task<IActionResult> GetSimilarByCategory([FromRoute] string category, [FromRoute] int limit)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category (famIntitule) is required.");
            }

            var similarProducts = await _produitsFunctions.GetSimilarProductByCategory(category, limit);

            if (similarProducts == null || !similarProducts.Any())
            {
                return NotFound("No similar products found in the given category.");
            }

            return Ok(similarProducts);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduit([FromBody] ArticleDto articledto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProduit = await _produitsFunctions.CreateProduit(articledto);
            return Ok(createdProduit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduit(int id, [FromBody] Article article)
        {
            if (id != article.ArtId)
                return BadRequest("Product ID mismatch");

            var updatedProduitsuccess = await _produitsFunctions.UpdateProduit(article);
         

            return Ok(updatedProduitsuccess);
        }
        [HttpPatch("updateFlag/{id}")]
        public async Task<IActionResult> UpdateProduitFlag(int id , [FromQuery] int flag)
        {
            await _produitsFunctions.UpdateProduitFlag(id, flag);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var success = await _produitsFunctions.DeleteProduit(id);
            if (!success)
                return NotFound($"Product with ID {id} not found");

            return NoContent();
        }
        [HttpGet("recommendations/{tiersCode}/{limit}")]
        public async Task<IActionResult> GetRecommendations([FromRoute] string tiersCode, [FromRoute] int limit)
        {
            if (string.IsNullOrWhiteSpace(tiersCode))
            {
                return BadRequest("Tiers code is required.");
            }
            var recommendations = await _produitsFunctions.GetRecommendations(tiersCode, limit);
            if (recommendations == null || !recommendations.Any())
            {
                return NotFound("No recommendations found for the given tiers code.");
            }
            return Ok(recommendations);
        }
        [HttpPost("recommendations/cart")]
        public async Task<IActionResult> GetProduitsByCodesLimited([FromBody] ArticlesByIdsRequest request)
        {
            var articles = await _produitsFunctions.GetProduitsByCodesLimited(request.item_ids, request.Count);
            return Ok(articles);
        }

    }

    public class ArticlesByIdsRequest
    {
        public List<string> item_ids { get; set; }
        public int Count { get; set; }
    }
}
