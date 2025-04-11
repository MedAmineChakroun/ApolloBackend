namespace ApolloBackend.Models.DTOs
{
    public class DocumentVenteLigneDto
    {
        public string LigneDocPiece { get; set; } = null!;

        public string? LigneArtCode { get; set; }

        public string? LigneArtDesi { get; set; }

        public decimal? LigneQte { get; set; }

        public decimal? LignePu { get; set; }

        public decimal? LigneHt { get; set; }

        public decimal? LigneTtc { get; set; }


    }
}
