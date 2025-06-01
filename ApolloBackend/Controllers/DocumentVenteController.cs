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
            envoiMail(tiersCode, title, msg, type);
            return Ok(updated);
            
         
        }

        [HttpPatch("updateFlag/{id}")]
        public async Task<IActionResult> UpdateDocumentFlag(int id, [FromQuery] int flag)
        {
           DocumentVente doc =  await _documentVenteService.UpdateDocumentFlag(id, flag);
            return Ok(doc);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocumentVente(int id, [FromBody] DocumentVenteDto dto)
        {
            var updated = await _documentVenteService.UpdateDocumentVente(id, dto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent(); // Ou return Ok() si tu veux renvoyer un résultat
        }
        private async Task envoiMail(string tiersCode, string title, string msg, string type)
        {
            try
            {
                var client = await _db.Clients.FirstOrDefaultAsync(c => c.TiersCode == tiersCode);
                if (client == null || string.IsNullOrWhiteSpace(client.TiersEmail))
                {
                    Console.WriteLine($"Client not found or email missing for tiersCode: {tiersCode}");
                    return;
                }

                // Validate email format
                try { var testEmail = new MailAddress(client.TiersEmail); }
                catch
                {
                    Console.WriteLine($"Invalid email format for client {tiersCode}: {client.TiersEmail}");
                    return;
                }

                var toAddress = new MailAddress(client.TiersEmail, client.TiersIntitule ?? "Client");
                var fromAddress = new MailAddress("hbnkii2@gmail.com", "Assistance Apollo Store");
                const string fromPassword = "cjck cnnd htvw qhfl";

                // Build HTML content
                string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif; font-size: 14px; color: #333;'>
                    <p>Bonjour <strong>{client.TiersIntitule ?? "Client"}</strong>,</p>
                    <p>{msg}</p>
                    <p>Si vous avez des questions, n'hésitez pas à nous contacter.</p>
                    <br />
                    <p style='color: #555;'>Cordialement,</p>
                    <p><strong>Assistance Apollo Store</strong><br />📧 scsi@contact.com<br />📞 +216 99 039 892</p>
                    <hr style='border: none; border-top: 1px solid #ccc;' />
                    <p style='font-size: 12px; color: #888;'>Ceci est un message automatique. Merci de ne pas y répondre.</p>
                </body>
            </html>";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 10000
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = title,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                // Optionally add a Reply-To or BCC
                // message.ReplyToList.Add("support@apollostore.com");

                var sendTask = smtp.SendMailAsync(message);
                if (await Task.WhenAny(sendTask, Task.Delay(15000)) == sendTask)
                {
                    Console.WriteLine($"✅ Email successfully sent to {client.TiersEmail}");
                }
                else
                {
                    Console.WriteLine($"⏱ Email sending timed out for {client.TiersEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to send email to client {tiersCode}: {ex.Message}");
            }
        }
        [HttpGet("isProductPurshased/{tiersCode}/{artCode}")]
        public async Task<ActionResult<bool>> IsProductPurshased(string tiersCode, string artCode)
        {
            var result = await _documentVenteService.isProductPurshased(tiersCode, artCode);
            return Ok(result);
        }
        [HttpGet("hasOrders/{tiersCode}")]
        public async Task<ActionResult<bool>> HasOrders(string tiersCode)
        {
            var result = await _documentVenteService.HasOrders(tiersCode);
            return Ok(result);
        }
        [HttpGet("hasOrdersForArticles/{artCode}")]
        public async Task<ActionResult<bool>> HasOrdersForArticles(string artCode)
        {
            var result = await _documentVenteService.HasOrdersForArticles(artCode);
            return Ok(result);
        }

    }


}
