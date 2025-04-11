using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class wish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TiersId = table.Column<int>(type: "int", nullable: false),
                    ArtId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Articles_ArtId",
                        column: x => x.ArtId,
                        principalTable: "Articles",
                        principalColumn: "ArtId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Clients_TiersId",
                        column: x => x.TiersId,
                        principalTable: "Clients",
                        principalColumn: "TiersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ArtId",
                table: "WishlistItems",
                column: "ArtId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_TiersId",
                table: "WishlistItems",
                column: "TiersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishlistItems");
        }
    }
}
