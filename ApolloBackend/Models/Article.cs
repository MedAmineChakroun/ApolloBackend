using Microsoft.AspNetCore.Authentication;

namespace ApolloBackend.Models
{
    public class Article
    {

        public int ArtId { get; set; }

        public string ArtCode { get; set; } = null!;

        public string? ArtIntitule { get; set; }

        public string? ArtFamille { get; set; }

        public decimal? ArtPrixVente { get; set; }

        public decimal? ArtPrixAchat { get; set; }

        public short? ArtEtat { get; set; }

        public string? ArtUnite { get; set; }

        public string? ArtImageUrl { get; set; }

        public decimal? ArtTvaTaux { get; set; }

        public int ArtFlag { get; set; } = 0;

        public DateTime ArtDateCreate { get; set; }

    }
}
