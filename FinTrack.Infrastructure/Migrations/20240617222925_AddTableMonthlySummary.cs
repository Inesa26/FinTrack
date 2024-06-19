using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Infrastructure.Migrations
{
    public partial class AddTableMonthlySummary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlySummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Income = table.Column<decimal>(type: "Money", nullable: false, defaultValue: 0m),
                    Expenses = table.Column<decimal>(type: "Money", nullable: false, defaultValue: 0m),
                    Balance = table.Column<decimal>(type: "Money", nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlySummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlySummary_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthlySummary_AccountId",
                table: "MonthlySummary",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlySummary");
        }
    }
}
