using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ApolloBackend.Models
{
    public class Notification
    {
        public int Id { get; set; }                             // Primary Key
        public string TiersCode { get; set; }                     // Foreign Key to Client      
        public string Title { get; set; }                       // Notification Title
        public string Message { get; set; }                
        public bool IsRead { get; set; } = false;   
        public string type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;  
    }

}
