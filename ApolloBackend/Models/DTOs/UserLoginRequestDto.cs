using System.ComponentModel.DataAnnotations;

namespace ApolloBackend.Models.DTOs
{
    public class UserLoginRequestDtos
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
