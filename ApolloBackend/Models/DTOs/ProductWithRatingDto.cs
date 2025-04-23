namespace ApolloBackend.Models.DTOs
{
    public class ProductWithRatingDto
    {
        public Article Article { get; set; }
        public double AverageRating { get; set; }
        public int RatingCount { get; set; }
    }
}