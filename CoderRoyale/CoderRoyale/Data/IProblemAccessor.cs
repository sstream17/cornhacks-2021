﻿using System.Threading.Tasks;

namespace CoderRoyale.Data
{
	public interface IProblemAccessor
	{
		Task<Problem> GetProblem(int id);

		Task<ExpectedInputsOutputsDTO> GetExpectedInputsOutputs(int problemId);
	}
}
