namespace ApolloBackend.Models.DTOs
{
    public class RatingCreateDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; } 
        public int Stars { get; set; }
    }

}
