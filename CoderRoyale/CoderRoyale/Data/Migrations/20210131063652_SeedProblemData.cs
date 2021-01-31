using Microsoft.EntityFrameworkCore.Migrations;

namespace CoderRoyale.Data.Migrations
{
	public partial class SeedProblemData : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// Two Sum Problem
			migrationBuilder.InsertData(
				table: "Problems",
				columns: new[] { "Title", "InputVariables", "Description" },
				values: new object[] { "Two Sum", "nums, target", "Given an array of integers `nums` and an integer `target`, return *indices of the two numbers such that they add up to `target`*.\n\nYou may assume that each input would have ***exactly one solution***, and you may not use the *same* element twice.\n\nYou can return the answer in any order.\n\n \n\n### Example 1:\n\n```\nInput: nums = [2,7,11,15], target = 9\nOutput: [0,1]\nOutput: Because nums[0] + nums[1] == 9, we return [0, 1].\n```\n\n### Example 2:\n\n```\nInput: nums = [3,2,4], target = 6\nOutput: [1,2]\n```\n\n### Example 3:\n\n```\nInput: nums = [3,3], target = 6\nOutput: [0,1]\n ```\n\n### Constraints:\n\n- `2 <= nums.length <= 103`\n- `-109 <= nums[i] <= 109`\n- `-109 <= target <= 109`\n- **Only one valid answer exists.**" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 1, "[2,7,11,15] 9", "[0,1]" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 1, "[3,2,4] 6", "[1,2]" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 1, "[3,3] 6", "[0,1]" });

			// Missing Number Problem
			migrationBuilder.InsertData(
				table: "Problems",
				columns: new[] { "Title", "InputVariables", "Description" },
				values: new object[] { "Missing Number", "nums", "Given an array `nums` containing `n` distinct numbers in the range `[0, n]`, return *the only number in the range that is missing from the array*.\n\n### Example 1:\n\n```\nInput: nums = [3,0,1]\nOutput: 2\nExplanation: n = 3 since there are 3 numbers, so all numbers are in the range [0,3]. 2 is the missing number in the range since it does not appear in nums.\n```\n\n### Example 2:\n\n```\nInput: nums = [0,1]\nOutput: 2\nExplanation: n = 2 since there are 2 numbers, so all numbers are in the range [0,2]. 2 is the missing number in the range since it does not appear in nums.\n```\n\n### Example 3:\n\n```\nInput: nums = [9,6,4,2,3,5,7,0,1]\nOutput: 8\nExplanation: n = 9 since there are 9 numbers, so all numbers are in the range [0,9]. 8 is the missing number in the range since it does not appear in nums.\n```\n\n### Example 4:\n\n```\nInput: nums = [0]\nOutput: 1\nExplanation: n = 1 since there is 1 number, so all numbers are in the range [0,1]. 1 is the missing number in the range since it does not appear in nums.\n ```\n\n### Constraints:\n\n- `n == nums.length`\n- `1 <= n <= 104`\n- `0 <= nums[i] <= n`\n- All the numbers of `nums` are **unique**." });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 2, "[3,0,1]", "2" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 2, "[0,1]", "2" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 2, "[9,6,4,2,3,5,7,0,1]", "8" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 2, "[0]", "1" });

			// Climbing Stairs Problem
			migrationBuilder.InsertData(
				table: "Problems",
				columns: new[] { "Title", "InputVariables", "Description" },
				values: new object[] { "Climbing Stairs", "n", "You are climbing a staircase. It takes `n` steps to reach the top.\n\nEach time you can either climb `1` or `2` steps. In *how many distinct ways can you climb to the top*?\n\n \n### Example 1:\n\n```\nInput: n = 2\nOutput: 2\nExplanation: There are two ways to climb to the top.\n1. 1 step + 1 step\n2. 2 steps\n```\n\n### Example 2:\n\n```\nInput: n = 3\nOutput: 3\nExplanation: There are three ways to climb to the top.\n1. 1 step + 1 step + 1 step\n2. 1 step + 2 steps\n3. 2 steps + 1 step\n```\n \n### Constraints:\n\n- `1 <= n <= 45`" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 3, "2", "2" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 3, "3", "3" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 3, "4", "5" });

			migrationBuilder.InsertData(
				table: "ExpectedInputsOutputs",
				columns: new[] { "ProblemId", "Input", "Output" },
				values: new object[] { 3, "5", "8" });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ExpectedInputsOutputs");

			migrationBuilder.DropTable(
				name: "Problems");

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
	}
}
