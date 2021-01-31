using Microsoft.EntityFrameworkCore.Migrations;

namespace CoderRoyale.Data.Migrations
{
    public partial class AddInputVariablesToProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InputVariables",
                table: "Problems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InputVariables",
                table: "Problems");
        }
    }
}
