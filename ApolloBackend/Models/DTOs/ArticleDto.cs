namespace ApolloBackend.Models.DTOs
{
    public class ArticleDto
    {

        public string ArtIntitule { get; set; }
        public string ArtFamille { get; set; }
        public string? ArtImageUrl { get; set; }
        public decimal ArtPrixAchat { get; set; }
        public decimal ArtPrixVente { get; set; }
        public decimal ArtTvaTaux { get; set; }
        public string ArtUnite { get; set; }


    }
}
