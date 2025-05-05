namespace ApolloBackend.Models.DTOs
{
    public class EmailMessage
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MessageType { get; set; } // e.g., "commande"

        // Additional fields you might want to include
        public string TiersCode { get; set; }
        public string DocumentReference { get; set; } // e.g., DocPiece
    }

}
