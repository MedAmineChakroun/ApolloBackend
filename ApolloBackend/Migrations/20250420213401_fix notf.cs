using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class fixnotf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Clients_TiersId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TiersId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "TiersId",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "TiersCode",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TiersCode",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "TiersId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TiersId",
                table: "Notifications",
                column: "TiersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Clients_TiersId",
                table: "Notifications",
                column: "TiersId",
                principalTable: "Clients",
                principalColumn: "TiersId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
