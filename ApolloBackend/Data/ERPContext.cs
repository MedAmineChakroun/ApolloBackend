using System;
using System.Collections.Generic;
using ApolloBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApolloBackend.Data;

public partial class ERPContext : DbContext
{
    public ERPContext()
    {
    }

    public ERPContext(DbContextOptions<ERPContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ListeArticle> ListeArticles { get; set; }

    public virtual DbSet<ListeCatalogue> ListeCatalogues { get; set; }

    public virtual DbSet<ListeClient> ListeClients { get; set; }

    public virtual DbSet<ListeDocumentsVente> ListeDocumentsVentes { get; set; }

    public virtual DbSet<ListeDocumentsVenteLigne> ListeDocumentsVenteLignes { get; set; }

    public virtual DbSet<ListeFamille> ListeFamilles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-J3AQUCM;Database=ALLANI;Trusted_Connection=True;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ListeArticle>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_ARTICLES");

            entity.Property(e => e.ArtBarcode)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("ART_BARCODE");
            entity.Property(e => e.ArtCatalogue1)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ART_CATALOGUE1");
            entity.Property(e => e.ArtCatalogue2)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ART_CATALOGUE2");
            entity.Property(e => e.ArtCatalogue3)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ART_CATALOGUE3");
            entity.Property(e => e.ArtCatalogue4)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ART_CATALOGUE4");
            entity.Property(e => e.ArtCode)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("ART_CODE");
            entity.Property(e => e.ArtEtat).HasColumnName("ART_ETAT");
            entity.Property(e => e.ArtFamille)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("ART_FAMILLE");
            entity.Property(e => e.ArtId).HasColumnName("ART_ID");
            entity.Property(e => e.ArtIntitule)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("ART_INTITULE");
            entity.Property(e => e.ArtPrixAchat)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("ART_PRIX_ACHAT");
            entity.Property(e => e.ArtPrixVente)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("ART_PRIX_VENTE");
            entity.Property(e => e.ArtStat1)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_STAT1");
            entity.Property(e => e.ArtStat2)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_STAT2");
            entity.Property(e => e.ArtStat3)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_STAT3");
            entity.Property(e => e.ArtStat4)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_STAT4");
            entity.Property(e => e.ArtStat5)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_STAT5");
            entity.Property(e => e.ArtSuiviStock).HasColumnName("ART_SUIVI_STOCK");
            entity.Property(e => e.ArtUnite)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("ART_UNITE");
        });

        modelBuilder.Entity<ListeCatalogue>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_CATALOGUE");

            entity.Property(e => e.CatalogueCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CATALOGUE_CODE");
            entity.Property(e => e.CatalogueId).HasColumnName("CATALOGUE_ID");
            entity.Property(e => e.CatalogueIntitule)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("CATALOGUE_INTITULE");
            entity.Property(e => e.CatalogueNiveau).HasColumnName("CATALOGUE_NIVEAU");
            entity.Property(e => e.CatalogueParentCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CATALOGUE_PARENT_CODE");
            entity.Property(e => e.CatalogueParentId).HasColumnName("CATALOGUE_PARENT_ID");
            entity.Property(e => e.CatalogueParentIntitule)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("CATALOGUE_PARENT_INTITULE");
        });

        modelBuilder.Entity<ListeClient>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_CLIENTS");

            entity.Property(e => e.TiersAdresse1)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_ADRESSE1");
            entity.Property(e => e.TiersAdresse2)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_ADRESSE2");
            entity.Property(e => e.TiersCode)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("TIERS_CODE");
            entity.Property(e => e.TiersCodePostal)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("TIERS_CODE_POSTAL");
            entity.Property(e => e.TiersColnom)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_COLNOM");
            entity.Property(e => e.TiersColprenom)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_COLPRENOM");
            entity.Property(e => e.TiersContact)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_CONTACT");
            entity.Property(e => e.TiersId).HasColumnName("TIERS_ID");
            entity.Property(e => e.TiersIntitule)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("TIERS_INTITULE");
            entity.Property(e => e.TiersMails)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("TIERS_MAILS");
            entity.Property(e => e.TiersMf)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TIERS_MF");
            entity.Property(e => e.TiersPays)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_PAYS");
            entity.Property(e => e.TiersTel1)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("TIERS_TEL1");
            entity.Property(e => e.TiersTel2)
                .HasMaxLength(21)
                .IsUnicode(false)
                .HasColumnName("TIERS_TEL2");
            entity.Property(e => e.TiersVille)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("TIERS_VILLE");
        });

        modelBuilder.Entity<ListeDocumentsVente>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_DOCUMENTS_VENTE");

            entity.Property(e => e.DocDate)
                .HasColumnType("datetime")
                .HasColumnName("DOC_DATE");
            entity.Property(e => e.DocDepd)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("DOC_DEPD");
            entity.Property(e => e.DocId).HasColumnName("DOC_ID");
            entity.Property(e => e.DocPiece)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("DOC_PIECE");
            entity.Property(e => e.DocRef)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("DOC_REF");
            entity.Property(e => e.DocTht)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("DOC_THT");
            entity.Property(e => e.DocTiersCode)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("DOC_TIERS_CODE");
            entity.Property(e => e.DocTiersIntitule)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("DOC_TIERS_INTITULE");
            entity.Property(e => e.DocTtc)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("DOC_TTC");
            entity.Property(e => e.DocType).HasColumnName("DOC_TYPE");
        });

        modelBuilder.Entity<ListeDocumentsVenteLigne>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_DOCUMENTS_VENTE_LIGNES");

            entity.Property(e => e.LigneArtCode)
                .HasMaxLength(19)
                .IsUnicode(false)
                .HasColumnName("LIGNE_ART_CODE");
            entity.Property(e => e.LigneArtDesi)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("LIGNE_ART_DESI");
            entity.Property(e => e.LigneDocDate)
                .HasColumnType("datetime")
                .HasColumnName("LIGNE_DOC_DATE");
            entity.Property(e => e.LigneDocPiece)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("LIGNE_DOC_PIECE");
            entity.Property(e => e.LigneDocReg)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("LIGNE_DOC_REG");
            entity.Property(e => e.LigneDocType).HasColumnName("LIGNE_DOC_TYPE");
            entity.Property(e => e.LigneHt)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("LIGNE_HT");
            entity.Property(e => e.LigneId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LIGNE_ID");
            entity.Property(e => e.LignePu)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("LIGNE_PU");
            entity.Property(e => e.LigneQte)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("LIGNE_QTE");
            entity.Property(e => e.LigneTtc)
                .HasColumnType("numeric(24, 6)")
                .HasColumnName("LIGNE_TTC");
        });

        modelBuilder.Entity<ListeFamille>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("LISTE_FAMILLES");

            entity.Property(e => e.FamCode)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("FAM_CODE");
            entity.Property(e => e.FamId)
                .ValueGeneratedOnAdd()
                .HasColumnName("FAM_ID");
            entity.Property(e => e.FamIntitule)
                .HasMaxLength(69)
                .IsUnicode(false)
                .HasColumnName("FAM_INTITULE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
