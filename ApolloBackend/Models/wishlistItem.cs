using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApolloBackend.Models
{
    public class WishlistItem
    {
        [Key]
        public int Id { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        // Foreign Key vers Client
        public int TiersId { get; set; }
        [ForeignKey("TiersId")]
        public Client? Client { get; set; }

        // Foreign Key vers Article
        public int ArtId { get; set; }
        [ForeignKey("ArtId")]
        public Article? Article { get; set; }

        // Constructeur sans paramètre (obligatoire pour EF)
        public WishlistItem()
        {
        }

        // Constructeur personnalisé
        public WishlistItem(int tiersId, Client client, int artId, Article article)
        {
            TiersId = tiersId;
            Client = client;
            ArtId = artId;
            Article = article;
            AddedDate = DateTime.UtcNow;
        }

    }

}