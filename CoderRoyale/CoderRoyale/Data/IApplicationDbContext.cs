using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoderRoyale.Data
{
	public interface IApplicationDbContext
	{
		DbSet<Problem> Problems { get; set; }

		DbSet<ExpectedInputOutput> ExpectedInputsOutputs { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
