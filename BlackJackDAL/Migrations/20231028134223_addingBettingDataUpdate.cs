using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackJackDAL.Migrations
{
    /// <inheritdoc />
    public partial class addingBettingDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coins",
                table: "Players",
                newName: "Coins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coins",
                table: "Players",
                newName: "coins");
        }
    }
}
