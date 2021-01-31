using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderRoyale.Data
{
	public class Problem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProblemId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string InputVariables { get; set; }

		public virtual ICollection<ExpectedInputOutput> ExpectedInputsOutputs { get; set; }
	}
}
