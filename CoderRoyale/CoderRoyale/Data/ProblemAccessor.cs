using System.Collections.Generic;
using System.Threading.Tasks;

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
	}
}
