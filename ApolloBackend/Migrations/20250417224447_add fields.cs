using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class addfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocEtat",
                table: "DocumentVentes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocFlag",
                table: "DocumentVentes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DocNote",
                table: "DocumentVentes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TiersDateCreate",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TiersEmail",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TiersFlag",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ArtTvaTaux",
                table: "Articles",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocEtat",
                table: "DocumentVentes");

            migrationBuilder.DropColumn(
                name: "DocFlag",
                table: "DocumentVentes");

            migrationBuilder.DropColumn(
                name: "DocNote",
                table: "DocumentVentes");

            migrationBuilder.DropColumn(
                name: "TiersDateCreate",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TiersEmail",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "TiersFlag",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ArtTvaTaux",
                table: "Articles");
        }
    }
}
