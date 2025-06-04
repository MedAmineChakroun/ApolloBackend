using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentVenteLigneController : ControllerBase
    {
        private readonly IDocumentVenteLigne _documentVenteLigneService;
        public DocumentVenteLigneController(IDocumentVenteLigne documentVenteLigneService)
        {
            _documentVenteLigneService = documentVenteLigneService;
        }
        [HttpGet("piece/{docPiece}")]
        public async Task<ActionResult<List<DocumentVenteLigne>>> GetDocumentVenteLigneByPiece(string docPiece)
        {
            var lignes = await _documentVenteLigneService.getByDocumentVenteLignePiece(docPiece);
            return Ok(lignes);
        }
        [HttpPost("create")]
        public async Task<ActionResult<DocumentVenteLigne>> CreateDocumentVenteLigne([FromBody] DocumentVenteLigneDto documentVenteligneDto)
        {
            var newLigne = await _documentVenteLigneService.createDocumentVenteLigne(documentVenteligneDto);
            return Ok(newLigne);
        }
        [HttpGet("piece/nb/{docPiece}")]
        public async Task<ActionResult<int>> GetDocumentVenteLigneNbByPiece(string docPiece)
        {
            var lignes = await _documentVenteLigneService.getByDocumentVenteLignePiece(docPiece);
            return Ok(lignes.Count); 
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetLignesNb()
        {
            var lignesNb = await _documentVenteLigneService.GetLignesNb();
            return Ok(lignesNb);
        }
        [HttpGet]
        public async Task<List<DocumentVenteLigne>> GetDocumentVenteLignes()
        {
            return await _documentVenteLigneService.getDocumentVenteLignes();
        }
        [HttpGet("top")]
        public async Task<List<DocumentVenteLigne>> GetTopDocumentVenteLignes()
        {
            return await _documentVenteLigneService.getTopDocumentVenteLignes();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentVenteLigne(int id, [FromBody] DocumentVenteLigneDto dto)
        {
            var updated = await _documentVenteLigneService.UpdateDocumentVenteLigne(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent(); // ou return Ok() si tu veux renvoyer un message
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentVenteLigne(int id)
        {
            var deleted = await _documentVenteLigneService.deleteDocumentVenteLigne(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent(); // 204
        }

        [HttpGet("top-products")]
        public async Task<ActionResult<List<object>>> GetTop10BestSellingProducts()
        {
            var topProducts = await _documentVenteLigneService.GetTop10BestSellingProducts();
            return Ok(topProducts);
        }

        [HttpGet("top-categories")]
        public async Task<ActionResult<List<object>>> GetTop4BestSellingCategories()
        {
            var topCategories = await _documentVenteLigneService.GetTop4BestSellingCategories();
            return Ok(topCategories);
        }


    }
}
