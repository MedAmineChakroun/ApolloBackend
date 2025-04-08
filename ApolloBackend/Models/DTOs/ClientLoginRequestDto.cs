using System.ComponentModel.DataAnnotations;

namespace ApolloBackend.Models.DTOs
{
    public class ClientLoginRequestDtos
    {

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
        
    }
}
