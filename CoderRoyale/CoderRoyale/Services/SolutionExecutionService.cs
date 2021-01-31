using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoderRoyale.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoderRoyale.Services
{
	public class SolutionExecutionService
	{
		private const string _returnCode = "@return:";

		public SolutionExecutionService(IHubContext<GameHub> hubContext)
		{
			GameHubContext = hubContext;
		}

		private IHubContext<GameHub> GameHubContext { get; }

		public async Task CheckSolution(string userId, string code)
		{
			var codeFile = WriteCodeToFile(userId, code);
			await Task.Run(() => ExecuteSolution(userId, codeFile, "9"));
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

		private HubConnection connection;
		private NavigationManager navigationManager;

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
				var index = outputData.IndexOf(_returnCode, userIdEndIndex, _returnCode.Length);
				userOutput = outputData[(index + _returnCode.Length)..];

				// Check if correct solution
			}
			catch (ArgumentOutOfRangeException)
			{
				// Else console output
				userOutput = outputData[userIdEndIndex..];
			}

			connection = new HubConnectionBuilder()
				.WithUrl(navigationManager.ToAbsoluteUri("/gamehub"))
				.Build();

			await connection.InvokeAsync("SendExecutionResults", userId, 24, userOutput);
			//await GameHubContext.Clients.All.SendAsync("SendExecutionResults", userId, 24, userOutput);
		}
	}
}
