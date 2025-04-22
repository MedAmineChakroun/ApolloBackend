using ApolloBackend.Functions;
using ApolloBackend.Models; // Assuming Product model is here
using ApolloBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        // 🔽 Create
        [HttpPost]
        public async Task<IActionResult> CreateProduit([FromBody] Article produit)
        {
            var created = await _produitsFunctions.CreateProduit(produit);
            return Ok(new JsonResponseData { Data = created, IsSuccess = true, Message = "Produit ajouté avec succès" });
        }

        // 🔽 Update
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduit(int id, [FromBody] Article produit)
        {
            if (id != produit.ArtId)
                return BadRequest("ID mismatch");

            var updated = await _produitsFunctions.UpdateProduit(produit);
            if (updated == null)
                return NotFound($"Produit avec ID {id} non trouvé");

            return Ok(new JsonResponseData { Data = updated, IsSuccess = true, Message = "Produit mis à jour avec succès" });
        }

        // 🔽 Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var result = await _produitsFunctions.DeleteProduit(id);
            if (!result)
                return NotFound($"Produit avec ID {id} non trouvé");

            return Ok(new JsonResponseData { IsSuccess = true, Message = "Produit supprimé avec succès" });
        }

    }
}
