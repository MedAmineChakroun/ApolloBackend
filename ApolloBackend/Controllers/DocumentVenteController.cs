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
  
    public class DocumentVenteController : ControllerBase
    {
        private readonly IDocumentVente _documentVenteService;
        public DocumentVenteController(IDocumentVente documentVenteService)
        {
            _documentVenteService = documentVenteService;
        }
        [HttpGet]
        public async Task<ActionResult<List<DocumentVente>>> GetAllDocumentVentes()
        {
            var documentVentes = await _documentVenteService.GetDocumentVentes();
            return Ok(documentVentes);
        }
        [HttpGet("client/{tiersCode}")]
        public async Task<ActionResult<List<DocumentVente>>> GetDocumentVentesByTiers(string tiersCode)
        {
            var documentVentes = await _documentVenteService.GetDocumentVentesByTiers(tiersCode);
            return Ok(documentVentes);
        }
        [HttpPost("create")]
        public async Task<ActionResult<DocumentVente>> CreateDocumentVente([FromBody] DocumentVenteDto documentVenteDto)
        {
            var newDocument = await _documentVenteService.CreateDocumentVente(documentVenteDto);
            return Ok(newDocument);
        }
        [HttpGet("piece/{pieceCode}")]
        public async Task<ActionResult<DocumentVente>> GetDocumentByPieceCode(string pieceCode)
        {
            var documentVente = await _documentVenteService.GetDocumentByPieceCode(pieceCode);
            if (documentVente == null)
            {
                return NotFound();
            }
            return Ok(documentVente);
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetNbDocumentVentes()
        {
            var nbDocumentVentes = await _documentVenteService.GetNbCommande();
            return Ok(nbDocumentVentes);
        }
        [HttpGet("count/thisWeek")]
        public async Task<ActionResult<int>> GetNbCommandeAddedLastweek()
        {
            var nbDocumentVentes = await _documentVenteService.GetNbCommandeAddedLastweek();
            return Ok(nbDocumentVentes);
        }
    }
}
