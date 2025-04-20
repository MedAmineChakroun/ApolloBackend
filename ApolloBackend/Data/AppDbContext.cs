using ApolloBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Famille> Familles { get; set; }
        public DbSet<DocumentVente> DocumentVentes { get; set; }
        public DbSet<DocumentVenteLigne> DocumentVenteLignes { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Notification> Notifications { get; set; } // Ajout de la DbSet pour Notification
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define sequences
            modelBuilder.HasSequence<int>("ClientCodeSequence")
                .StartsAt(401000)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("ArticleCodeSequence")
                .StartsAt(101000)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("FamilleCodeSequence")
                .StartsAt(201000)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("DocumentCodeSequence")
                .StartsAt(301000)
                .IncrementsBy(1);

            // Identity configurations
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            // Client configuration
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.TiersId);
                entity.Property(e => e.TiersCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("FORMAT(NEXT VALUE FOR ClientCodeSequence, '0000000')");
                entity.HasIndex(e => e.TiersCode).IsUnique();
            });

            modelBuilder.Entity<User>()
        .HasOne(u => u.Client)
        .WithMany() // No navigation property back to User in Client
        .HasForeignKey(u => u.ClientId)
        .OnDelete(DeleteBehavior.Cascade);

            // Article configuration
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.ArtId);
                entity.Property(e => e.ArtCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("FORMAT(NEXT VALUE FOR ArticleCodeSequence, '0000000')");
                entity.HasIndex(e => e.ArtCode).IsUnique();
            });

            // Famille configuration
            modelBuilder.Entity<Famille>(entity =>
            {
                entity.HasKey(e => e.FamId);
                entity.Property(e => e.FamCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("FORMAT(NEXT VALUE FOR FamilleCodeSequence, '0000000')");
                entity.HasIndex(e => e.FamCode).IsUnique();
            });

            // DocumentVente configuration
            modelBuilder.Entity<DocumentVente>(entity =>
            {
                entity.HasKey(e => e.DocId);
                entity.Property(e => e.DocPiece)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("FORMAT(NEXT VALUE FOR DocumentCodeSequence, '0000000')");
                entity.HasIndex(e => e.DocPiece).IsUnique();
            });

            // DocumentVenteLigne configuration
            modelBuilder.Entity<DocumentVenteLigne>(entity =>
            {
                entity.HasKey(e => e.LigneId);
                entity.Property(e => e.LigneDocPiece)
                    .IsRequired()
                    .HasMaxLength(10);
                entity.HasOne<DocumentVente>()
                    .WithMany()
                    .HasForeignKey(e => e.LigneDocPiece)
                    .HasPrincipalKey(e => e.DocPiece)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            // Configuration de la relation WishlistItem → Client
            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.Client)
                .WithMany() // ou .WithMany(c => c.WishlistItems) si tu veux naviguer côté Client
                .HasForeignKey(w => w.TiersId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuration de la relation WishlistItem → Article
            modelBuilder.Entity<WishlistItem>()
                .HasOne(w => w.Article)
                .WithMany() // ou .WithMany(a => a.WishlistItems) si tu veux naviguer côté Article
                .HasForeignKey(w => w.ArtId)
                .OnDelete(DeleteBehavior.Cascade);
           
        }
    }
}