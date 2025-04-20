using ApolloBackend.Data;
using ApolloBackend.Functions;
using ApolloBackend.Interfaces;
using ApolloBackend.Models;
using ApolloBackend.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class DocumentVenteController : ControllerBase
    {
        private readonly IDocumentVente _documentVenteService;
        private readonly INotification _notificationService;

        public DocumentVenteController(
    IDocumentVente documentVenteService,
    INotification notificationservice,
    UserManager<User> userManager,
    AppDbContext db)
        {
            _documentVenteService = documentVenteService;
            _notificationService = notificationservice;

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
            newDocument.DocEtat = 0;
            newDocument.DocFlag = 0; 
            
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var result = await _documentVenteService.DeleteCommande(id);
            if (!result)
                return NotFound(new { message = $"No document found with ID {id}" });

            return Ok(new { message = $"Document with ID {id} deleted successfully" });
        }
        [HttpPatch("updateEtat/{id}")]
        public async Task<IActionResult> UpdateDocumentEtat(int id, [FromQuery] int etat, [FromQuery] string note)
        {
            var updated = await _documentVenteService.UpdateDocumentEtat(id, etat, note);
            var tiersCode = updated.DocTiersCode;
            var docPiece = updated.DocPiece;

            string title = "";
            string msg = "";
            string type = "commande";
            switch (etat)
            {
                case 0:
                    title = "Commande en attente";
                    msg = $"Votre commande #{docPiece} est en attente de traitement.";
                    break;
                case 1:
                    title = "Commande acceptée";
                    msg = $"Votre commande #{docPiece} a été acceptée et est en cours de préparation.";
                    break;
                case 2:
                    title = "Commande refusée";
                    msg = $"Votre commande #{docPiece} a été refusée. Veuillez vérifier les détails ou nous contacter.";
                    break;
                default:
                    title = "Mise à jour de commande";
                    msg = $"Votre commande #{docPiece} a été mise à jour.";
                    break;
            }

            await _notificationService.AddNotificationAsync(tiersCode, title, msg,type);
            return Ok(updated);
        }

        [HttpPatch("updateFlag/{id}")]
        public async Task<IActionResult> UpdateDocumentFlag(int id, [FromQuery] int flag)
        {
            var updated = await _documentVenteService.UpdateDocumentFlag(id, flag);
            return Ok(updated);
        }



    }
}
