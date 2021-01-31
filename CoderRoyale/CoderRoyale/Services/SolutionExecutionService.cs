using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CoderRoyale.Services
{
	public class SolutionExecutionService
	{
		public async Task CheckSolution(string code)
		{
			var codeFile = WriteCodeToFile(code);
			await Task.Run(() => ExecuteSolution(codeFile, "9"));
			File.Delete(codeFile);
		}

		private string WriteCodeToFile(string codeSolution)
		{
			var fileToWrite = $"{Directory.GetCurrentDirectory()}\\{Guid.NewGuid()}.py";
			var code = $"import sys\n\ndef solution(num):\n{codeSolution}\n\nprint(solution(sys.argv[1]))";
			File.WriteAllText(fileToWrite, code);
			return fileToWrite;
		}

		private void ExecuteSolution(string codeFile, string methodInput)
		{
			var processInfo = new ProcessStartInfo()
			{
				FileName = "docker",
				Arguments = $@"run -v {codeFile}:/code.py -i docker-code {methodInput}",
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
			Console.WriteLine(outLine);
		}
	}
}
