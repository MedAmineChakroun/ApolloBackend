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
        public DbSet<Article> Articles { get; set; }
        public DbSet<Famille> Familles { get; set; }
        public DbSet<DocumentVente> DocumentVentes { get; set; }
        public DbSet<DocumentVenteLigne> DocumentVenteLignes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define sequences
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
                      .IsRequired()  // Added IsRequired() for consistency
                      .HasMaxLength(10);
                entity.HasOne<DocumentVente>()
                      .WithMany()
                      .HasForeignKey(e => e.LigneDocPiece)
                      .HasPrincipalKey(e => e.DocPiece)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
