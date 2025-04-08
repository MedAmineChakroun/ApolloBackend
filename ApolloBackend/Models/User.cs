using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApolloBackend.Models
{
    public class User : IdentityUser
    {
        [Required]
        [EmailAddress]
        public override string Email { get; set; }


        // Foreign keys (only one will be used per user)
        public int? ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }

}
