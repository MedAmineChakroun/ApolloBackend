using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class addflaganddatetoarticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArtDateCreate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ArtFlag",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtDateCreate",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArtFlag",
                table: "Articles");
        }
    }
}
