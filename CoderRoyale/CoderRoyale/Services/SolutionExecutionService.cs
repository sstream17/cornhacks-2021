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

		public async Task CheckSolution(string userId, string code, int problemId, string inputVariables)
		{
			var codeFile = WriteCodeToFile(userId, code, inputVariables);
			var inputsOutputs = await ProblemAccessor.GetExpectedInputsOutputs(problemId);
			await Task.Run(() => ExecuteSolution(codeFile, inputsOutputs));
			File.Delete(codeFile);
		}

		private string WriteCodeToFile(string userId, string codeSolution, string inputVariables)
		{
			var sanitizedCodeSolution = Regex.Replace(codeSolution, "\t", "    ");
			var fileToWrite = $"{Directory.GetCurrentDirectory()}\\Drop\\{Guid.NewGuid()}.py";
			var code =
$@"import sys

args = []
for i in range(1, len(sys.argv)):
    arg = sys.argv[i]
    if arg[0] == '[' and arg[-1] == ']':
        arg = arg[1:-1]
        arg = arg.split(',')
    args.append(arg)

_print = print


def print(*args, **kw):
    args = (f'@{userId}:{{arg}}' for arg in args)
    _print(*args, **kw)


def solution({inputVariables}):
{sanitizedCodeSolution}


print(f'@return:{{solution(*args)}}')";
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
				if (index == -1)
				{
					throw new ArgumentOutOfRangeException();
				}

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
