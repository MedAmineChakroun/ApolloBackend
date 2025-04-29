using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApolloBackend.Migrations
{
    /// <inheritdoc />
    public partial class seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DBCC CHECKIDENT ('Articles', RESEED, 99999);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Clients', RESEED, 99999);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('DocumentVentes', RESEED, 99999);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('DocumentVenteLignes', RESEED, 99999);");
            migrationBuilder.Sql("DBCC CHECKIDENT ('Familles', RESEED, 99999);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
