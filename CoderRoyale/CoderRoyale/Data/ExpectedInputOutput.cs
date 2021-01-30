using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoderRoyale.Data
{
	public class ExpectedInputOutput
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ExpectedInputOutputId { get; set; }
		public string Input { get; set; }
		public string Output { get; set; }

		public int ProblemId { get; set; }
		public Problem Problem { get; set; }
	}
}
