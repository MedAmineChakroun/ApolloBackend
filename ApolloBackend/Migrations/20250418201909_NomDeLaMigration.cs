using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class NomDeLaMigration : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "TiersFlag",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
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
                name: "TiersFlag",
                table: "Clients");
        }
    }
}
