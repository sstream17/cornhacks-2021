using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoderRoyale.Data
{
	public class ProblemAccessor : IProblemAccessor
	{
		public ProblemAccessor(IApplicationDbContext dbContext)
		{
			DbContext = dbContext;
		}

		private IApplicationDbContext DbContext { get; }

		public async Task<Problem> GetProblem(int id)
		{
			try
			{
				var problem = await DbContext
					.Problems
					.FindAsync(id)
					.ConfigureAwait(false);

				if (problem == null)
				{
					throw new KeyNotFoundException($"Problem with Id {id} was not found.");
				}

				return problem;
			}
			catch (KeyNotFoundException)
			{
				throw;
			}
		}

		public async Task<ExpectedInputsOutputsDTO> GetExpectedInputsOutputs(int problemId)
		{
			try
			{
				var inputsOutputs = await DbContext
					.ExpectedInputsOutputs
					.Where(e => e.Problem.ProblemId == problemId)
					.ToListAsync()
					.ConfigureAwait(false);

				if (inputsOutputs == null)
				{
					throw new KeyNotFoundException($"No expected inputs or outputs for {problemId} were found.");
				}

				var inputs = new List<string>();
				var outputs = new List<string>();

				foreach (var inputOutput in inputsOutputs)
				{
					inputs.Add(inputOutput.Input);
					outputs.Add(inputOutput.Output);
				}

				return new ExpectedInputsOutputsDTO
				{
					Inputs = inputs,
					Outputs = outputs
				};
			}
			catch (KeyNotFoundException)
			{
				throw;
			}
		}
	}
}
