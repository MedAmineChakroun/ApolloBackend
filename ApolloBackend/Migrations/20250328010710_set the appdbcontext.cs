using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class settheappdbcontext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ArticleCodeSequence",
                startValue: 101000L);

            migrationBuilder.CreateSequence<int>(
                name: "DocumentCodeSequence",
                startValue: 301000L);

            migrationBuilder.CreateSequence<int>(
                name: "FamilleCodeSequence",
                startValue: 201000L);

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArtId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArtCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "FORMAT(NEXT VALUE FOR ArticleCodeSequence, '0000000')"),
                    ArtIntitule = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtFamille = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtPrixVente = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArtPrixAchat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArtEtat = table.Column<short>(type: "smallint", nullable: true),
                    ArtUnite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArtId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVentes",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocType = table.Column<short>(type: "smallint", nullable: true),
                    DocPiece = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "FORMAT(NEXT VALUE FOR DocumentCodeSequence, '0000000')"),
                    DocDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DocTht = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DocTtc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DocTiersCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocTiersIntitule = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVentes", x => x.DocId);
                    table.UniqueConstraint("AK_DocumentVentes_DocPiece", x => x.DocPiece);
                });

            migrationBuilder.CreateTable(
                name: "Familles",
                columns: table => new
                {
                    FamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FamCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "FORMAT(NEXT VALUE FOR FamilleCodeSequence, '0000000')"),
                    FamIntitule = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familles", x => x.FamId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVenteLignes",
                columns: table => new
                {
                    LigneId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LigneDocPiece = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LigneArtCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LigneArtDesi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LigneQte = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LignePu = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LigneHt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LigneTtc = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVenteLignes", x => x.LigneId);
                    table.ForeignKey(
                        name: "FK_DocumentVenteLignes_DocumentVentes_LigneDocPiece",
                        column: x => x.LigneDocPiece,
                        principalTable: "DocumentVentes",
                        principalColumn: "DocPiece",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArtCode",
                table: "Articles",
                column: "ArtCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVenteLignes_LigneDocPiece",
                table: "DocumentVenteLignes",
                column: "LigneDocPiece");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVentes_DocPiece",
                table: "DocumentVentes",
                column: "DocPiece",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Familles_FamCode",
                table: "Familles",
                column: "FamCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "DocumentVenteLignes");

            migrationBuilder.DropTable(
                name: "Familles");

            migrationBuilder.DropTable(
                name: "DocumentVentes");

            migrationBuilder.DropSequence(
                name: "ArticleCodeSequence");

            migrationBuilder.DropSequence(
                name: "DocumentCodeSequence");

            migrationBuilder.DropSequence(
                name: "FamilleCodeSequence");
        }
    }
}
