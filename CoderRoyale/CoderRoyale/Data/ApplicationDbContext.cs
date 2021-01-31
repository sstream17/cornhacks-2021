using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoderRoyale.Data
{
	public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public virtual DbSet<Problem> Problems { get; set; }

		public virtual DbSet<ExpectedInputOutput> ExpectedInputsOutputs { get; set; }
	}
}
