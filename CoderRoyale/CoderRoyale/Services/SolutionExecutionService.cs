using System.Diagnostics;
using System.Threading.Tasks;

namespace CoderRoyale.Services
{
	public class SolutionExecutionService
	{
		public async Task CheckSolution(string code)
		{
			await Task.Run(ExecuteSolution);
		}

		private void ExecuteSolution()
		{
			var processInfo = new ProcessStartInfo()
			{
				FileName = "docker",
				Arguments = @"run -v C:\Users\sprew\cornhacks-2021\inputs\yeet.py:/code.py -i docker-code nice",
				CreateNoWindow = true,
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				WindowStyle = ProcessWindowStyle.Hidden
			};

			var process = new Process
			{
				StartInfo = processInfo
			};

			process.OutputDataReceived += new DataReceivedEventHandler(ReadData);

			process.Start();
			process.BeginOutputReadLine();
			process.WaitForExit();
			process.Close();
		}

		private void ReadData(
			object sendingProcess,
			DataReceivedEventArgs outLine)
		{
			System.Console.WriteLine(outLine);
		}
	}
}
