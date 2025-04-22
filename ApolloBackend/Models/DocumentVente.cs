namespace ApolloBackend.Models
{
    public class DocumentVente
    {
        public short? DocType { get; set; }

        public string? DocPiece { get; set; }

        public DateTime? DocDate { get; set; }

        public decimal? DocTht { get; set; }

        public decimal? DocTtc { get; set; }

        public string? DocTiersCode { get; set; }

        public string? DocTiersIntitule { get; set; }

        public int DocId { get; set; }

        public int? DocEtat { get; set; }

        public string? DocNote { get; set; }

        public int? DocFlag{ get; set; }

    }
}
