﻿// <auto-generated />
using System;
using ApolloBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApolloBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250402001320_FixUserClientRelationship")]
    partial class FixUserClientRelationship
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.HasSequence<int>("ArticleCodeSequence")
                .StartsAt(101000L);

            modelBuilder.HasSequence<int>("ClientCodeSequence")
                .StartsAt(401000L);

            modelBuilder.HasSequence<int>("DocumentCodeSequence")
                .StartsAt(301000L);

            modelBuilder.HasSequence<int>("FamilleCodeSequence")
                .StartsAt(201000L);

            modelBuilder.Entity("ApolloBackend.Models.Article", b =>
                {
                    b.Property<int>("ArtId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtId"));

                    b.Property<string>("ArtCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValueSql("FORMAT(NEXT VALUE FOR ArticleCodeSequence, '0000000')");

                    b.Property<short?>("ArtEtat")
                        .HasColumnType("smallint");

                    b.Property<string>("ArtFamille")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArtImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArtIntitule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("ArtPrixAchat")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("ArtPrixVente")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ArtUnite")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtId");

                    b.HasIndex("ArtCode")
                        .IsUnique();

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("ApolloBackend.Models.Client", b =>
                {
                    b.Property<int>("TiersId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TiersId"));

                    b.Property<string>("TiersAdresse1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TiersCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValueSql("FORMAT(NEXT VALUE FOR ClientCodeSequence, '0000000')");

                    b.Property<string>("TiersCodePostal")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TiersIntitule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TiersPays")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TiersTel1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TiersVille")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TiersId");

                    b.HasIndex("TiersCode")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ApolloBackend.Models.DocumentVente", b =>
                {
                    b.Property<int>("DocId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocId"));

                    b.Property<DateTime?>("DocDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocPiece")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValueSql("FORMAT(NEXT VALUE FOR DocumentCodeSequence, '0000000')");

                    b.Property<decimal?>("DocTht")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("DocTiersCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocTiersIntitule")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DocTtc")
                        .HasColumnType("decimal(18,2)");

                    b.Property<short?>("DocType")
                        .HasColumnType("smallint");

                    b.HasKey("DocId");

                    b.HasIndex("DocPiece")
                        .IsUnique();

                    b.ToTable("DocumentVentes");
                });

            modelBuilder.Entity("ApolloBackend.Models.DocumentVenteLigne", b =>
                {
                    b.Property<int>("LigneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LigneId"));

                    b.Property<string>("LigneArtCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LigneArtDesi")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LigneDocPiece")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal?>("LigneHt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("LignePu")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("LigneQte")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("LigneTtc")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("LigneId");

                    b.HasIndex("LigneDocPiece");

                    b.ToTable("DocumentVenteLignes");
                });

            modelBuilder.Entity("ApolloBackend.Models.Famille", b =>
                {
                    b.Property<int>("FamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FamId"));

                    b.Property<string>("FamCode")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasDefaultValueSql("FORMAT(NEXT VALUE FOR FamilleCodeSequence, '0000000')");

                    b.Property<string>("FamIntitule")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FamId");

                    b.HasIndex("FamCode")
                        .IsUnique();

                    b.ToTable("Familles");
                });

            modelBuilder.Entity("ApolloBackend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "ProviderKey");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ApolloBackend.Models.Client", b =>
                {
                    b.HasOne("ApolloBackend.Models.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("ApolloBackend.Models.Client", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("ApolloBackend.Models.DocumentVenteLigne", b =>
                {
                    b.HasOne("ApolloBackend.Models.DocumentVente", null)
                        .WithMany()
                        .HasForeignKey("LigneDocPiece")
                        .HasPrincipalKey("DocPiece")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ApolloBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ApolloBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApolloBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ApolloBackend.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApolloBackend.Models.User", b =>
                {
                    b.Navigation("Client");
                });
#pragma warning restore 612, 618
        }
    }
}
