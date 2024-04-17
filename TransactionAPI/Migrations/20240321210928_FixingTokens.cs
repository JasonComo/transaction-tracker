using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixingTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AspNetUsers",
                newName: "FullName");
        }
    }
}
