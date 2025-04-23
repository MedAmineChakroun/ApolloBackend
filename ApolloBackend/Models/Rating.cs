using ApolloBackend.Models;
using System;
using System.Text.Json.Serialization;

namespace ApolloBackend.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Article Product { get; set; } = null!;
        public int UserId { get; set; }
        [JsonIgnore]
        public Client client { get; set; } = null!;
        public int Stars { get; set; }
        public DateTime CreatedAt { get; set; }


    }

}