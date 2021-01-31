using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CoderRoyale.Services
{
	public class SolutionExecutionService
	{
		public async Task CheckSolution(string userId, string code)
		{
			var codeFile = WriteCodeToFile(userId, code);
			await Task.Run(() => ExecuteSolution(userId, codeFile, "9"));
			File.Delete(codeFile);
		}

		private string WriteCodeToFile(string userId, string codeSolution)
		{
			var fileToWrite = $"{Directory.GetCurrentDirectory()}\\Build\\{Guid.NewGuid()}.py";
			var code =
$@"import sys

_print = print
def print(*args, **kw):
    args = (f'@{userId}:{{arg}}' for arg in args)
    _print(*args, **kw)

def solution(num):
{codeSolution}

print(solution(sys.argv[1]))";
			File.WriteAllText(fileToWrite, code);
			return fileToWrite;
		}

		private void ExecuteSolution(string userId, string codeFile, string methodInput)
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
			process.ErrorDataReceived += new DataReceivedEventHandler(ReadData);

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
