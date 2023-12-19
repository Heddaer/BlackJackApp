using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlackJackDAL.Migrations
{
    /// <inheritdoc />
    public partial class addingBettingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "coins",
                table: "Players",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "BettingData",
                columns: table => new
                {
                    BettingDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bet = table.Column<double>(type: "float", nullable: false),
                    PlayerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BettingData", x => x.BettingDataID);
                    table.ForeignKey(
                        name: "FK_BettingData_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BettingData_PlayerID",
                table: "BettingData",
                column: "PlayerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BettingData");

            migrationBuilder.DropColumn(
                name: "coins",
                table: "Players");
        }
    }
}
