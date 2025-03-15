using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApolloBackend.Models
{
    public class User:IdentityUser
    {
        public override string Email { get; set; }
    }
}
