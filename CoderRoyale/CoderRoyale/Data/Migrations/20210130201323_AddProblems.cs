using Microsoft.EntityFrameworkCore.Migrations;

namespace CoderRoyale.Data.Migrations
{
    public partial class AddProblems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    ProblemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.ProblemId);
                });

            migrationBuilder.CreateTable(
                name: "ExpectedInputsOutputs",
                columns: table => new
                {
                    ExpectedInputOutputId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Input = table.Column<string>(nullable: true),
                    Output = table.Column<string>(nullable: true),
                    ProblemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpectedInputsOutputs", x => x.ExpectedInputOutputId);
                    table.ForeignKey(
                        name: "FK_ExpectedInputsOutputs_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "ProblemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpectedInputsOutputs_ProblemId",
                table: "ExpectedInputsOutputs",
                column: "ProblemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpectedInputsOutputs");

            migrationBuilder.DropTable(
                name: "Problems");
        }
    }
}
