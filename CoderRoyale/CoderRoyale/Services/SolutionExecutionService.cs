using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoderRoyale.Data;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoderRoyale.Services
{
	public class SolutionExecutionService : ISolutionExecutionService
	{
		private const string _returnCode = "@return:";

		private readonly HubConnection connection;

		public SolutionExecutionService(IProblemAccessor problemAccessor)
		{
			ProblemAccessor = problemAccessor;

			connection = new HubConnectionBuilder()
				.WithUrl("https://localhost:44316/gamehub")
				.Build();

			connection.StartAsync();
		}

		private IProblemAccessor ProblemAccessor { get; }

		public async Task CheckSolution(string userId, string code, int problemId)
		{
			var codeFile = WriteCodeToFile(userId, code);
			var inputsOutputs = await ProblemAccessor.GetExpectedInputsOutputs(problemId);
			await Task.Run(() => ExecuteSolution(codeFile, inputsOutputs));
			File.Delete(codeFile);
		}

		private string WriteCodeToFile(string userId, string codeSolution)
		{
			var sanitizedCodeSolution = Regex.Replace(codeSolution, "\t", "    ");
			var fileToWrite = $"{Directory.GetCurrentDirectory()}\\Drop\\{Guid.NewGuid()}.py";
			var code =
$@"import sys

_print = print
def print(*args, **kw):
    args = (f'@{userId}:{{arg}}' for arg in args)
    _print(*args, **kw)

def solution(num):
{sanitizedCodeSolution}

print(f'@return:{{solution(sys.argv[1])}}')";
			File.WriteAllText(fileToWrite, code);
			return fileToWrite;
		}

		private void ExecuteSolution(string codeFile, ExpectedInputsOutputsDTO inputsOutputs)
		{
			for (var i = 0; i < inputsOutputs.Inputs.Count; i++)
			{
				var inputs = inputsOutputs.Inputs[i];
				var processInfo = new ProcessStartInfo()
				{
					FileName = "docker",
					Arguments = $@"run --rm -v {codeFile}:/code.py -i docker-code {inputs}",
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
		}

		private async void ReadData(
			object sendingProcess,
			DataReceivedEventArgs outLine)
		{
			if (outLine.Data == null)
			{
				return;
			}

			var outputData = outLine.Data;
			var userIdEndIndex = outputData.IndexOf(":");
			var userId = outputData[1..userIdEndIndex];
			string userOutput;
			try
			{
				// If returned code
				var index = outputData.IndexOf(_returnCode, userIdEndIndex, _returnCode.Length + 1);
				userOutput = outputData[(index + _returnCode.Length)..];

				// Check if correct solution
			}
			catch (ArgumentOutOfRangeException)
			{
				// Else console output
				userOutput = outputData[(userIdEndIndex + 1)..];
			}

			await connection.InvokeAsync("SendExecutionResults", userId, 24, userOutput);
		}
	}
}
