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
using System.Net.Mail;
using System.Net;


namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class DocumentVenteController : ControllerBase
    {
        private readonly IDocumentVente _documentVenteService;
        private readonly INotification _notificationService;
        private readonly AppDbContext _db;

        public DocumentVenteController(
            IDocumentVente documentVenteService,
            INotification notificationservice,
            AppDbContext db)
        {
            _documentVenteService = documentVenteService;
            _notificationService = notificationservice;
            _db = db;
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
            
                
        

            await _notificationService.AddNotificationAsync(tiersCode, title, msg, type);
            _ = Task.Run(() => envoiMail(tiersCode, title, msg, type));
            return Ok(updated);
            
         
        }

        [HttpPatch("updateFlag/{id}")]
        public async Task<IActionResult> UpdateDocumentFlag(int id, [FromQuery] int flag)
        {
            var updated = await _documentVenteService.UpdateDocumentFlag(id, flag);
            return Ok(updated);
        }

        private async Task envoiMail(string tiersCode, string title, string msg, string type)
        {
            try
            {
                // Find client email from ERP
                var client = await _db.Clients.FirstOrDefaultAsync(c => c.TiersCode == tiersCode);
                if (client == null || string.IsNullOrWhiteSpace(client.TiersEmail))
                    return; // No email found, just exit silently

                // Validate email address format
                try
                {
                    var emailAddress = new MailAddress(client.TiersEmail);
                }
                catch
                {
                    // Invalid email format, just exit silently
                    Console.WriteLine($"Invalid email format for client {tiersCode}: {client.TiersEmail}");
                    return;
                }

                var toAddress = new MailAddress(client.TiersEmail, client.TiersIntitule ?? "Client");
                var fromAddress = new MailAddress("hbnkii2@gmail.com", "Assistance Apollo Store");
                const string fromPassword = "cjck cnnd htvw qhfl";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 10000 // 10 second timeout to prevent hanging
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = title,
                    Body = msg,
                    IsBodyHtml = false
                })
                {
                    // Use a task with timeout to prevent blocking indefinitely
                    var sendTask = smtp.SendMailAsync(message);

                    // Wait for the task to complete or timeout after 15 seconds
                    if (await Task.WhenAny(sendTask, Task.Delay(15000)) == sendTask)
                    {
                        // Email sent successfully
                        Console.WriteLine($"Email sent successfully to {client.TiersEmail}");
                    }
                    else
                    {
                        // Email sending timed out
                        Console.WriteLine($"Email sending timed out for {client.TiersEmail}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception but don't throw - allow the application to continue
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

    }


}
