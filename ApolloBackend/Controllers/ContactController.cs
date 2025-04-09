using ApolloBackend.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace ApolloBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail([FromBody] ContactForm form)
        {
            try
            {
                const string fromPassword = "cjck cnnd htvw qhfl"; 
                var toAdress = new MailAddress("hbnkii2@gmail.com","Assistance");
                
                var fromAdress = new MailAddress("hbnkii2@gmail.com", "Client");
                var subject = form.Subject;
                var content = form.Message;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAdress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAdress, toAdress)
                {
                    Subject = subject,
                    Body = $"From: {form.Email}\n\n{form.Message}"
                })
                {
                    message.ReplyToList.Add(new MailAddress(form.Email));
                    smtp.Send(message);
                }

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
