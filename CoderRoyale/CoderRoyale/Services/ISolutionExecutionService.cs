using System.Threading.Tasks;

namespace CoderRoyale.Services
{
	public interface ISolutionExecutionService
	{
		Task CheckSolution(string userId, string code, int problemId, string inputVariables);
	}
}
