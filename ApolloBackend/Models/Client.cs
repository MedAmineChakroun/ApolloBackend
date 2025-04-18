namespace ApolloBackend.Models
{
    public class Client
    {
        public int TiersId { get; set; }

        public string TiersCode { get; set; } = null!;

        public string? TiersIntitule { get; set; }

        public string? TiersAdresse1 { get; set; }

        public string? TiersCodePostal { get; set; }

        public string? TiersVille { get; set; }

        public string? TiersPays { get; set; }

        public string? TiersTel1 { get; set; }

        public int TiersFlag { get; set; }
        public DateTime? TiersDateCreate { get; set; }

        public string? TiersEmail { get; set; }

    }
}
