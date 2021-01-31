using System.Threading.Tasks;
using System.Timers;
using CoderRoyale.Data;
using Markdig;
using Microsoft.AspNetCore.SignalR;

namespace CoderRoyale.Hubs
{
	public class GameHub : Hub
	{
		public GameHub(IProblemAccessor problemAccessor)
		{
			ProblemAccessor = problemAccessor;
		}

		private IProblemAccessor ProblemAccessor { get; }

		public async Task SendExecutionResults(
			string submittedUser,
			int timeSubmitted,
			string output)
		{
			await Clients.All.SendAsync(
				"ReceiveExecutionResults",
				submittedUser,
				timeSubmitted,
				output);
		}

		public async Task PlayerJoined()
		{
			await Clients.All.SendAsync("PlayerJoined", 1);
		}

		public async Task StartGame()
		{
			await Clients.All.SendAsync("StartGame", 1);
			await SendNextRound(1, 10);
		}

		public async Task SendEndGame()
		{
			await Clients.All.SendAsync("ReceiveEndGame");
		}

		public async Task SendPlayerComplete(string successfulPlayer)
		{
			await Clients.All.SendAsync("ReceivePlayerComplete", successfulPlayer);
		}

		public async Task SendPlayerDisconnect(string disconnectedPlayer)
		{
			await Clients.All.SendAsync("ReceivePlayerDisconnect", disconnectedPlayer);
		}

		public async Task SendRemainingTime(int remainingTime)
		{
			await Clients.All.SendAsync("ReceiveRemainingTime", remainingTime);
		}

		public async Task SendNextRound(int roundNumber, int numberOfPlayersToAdvance)
		{
			var problem = await ProblemAccessor.GetProblem((roundNumber % 3) + 1);
			problem.Description = Markdown.ToHtml(problem.Description);
			var secondsRemaining = 300;
			await Clients.All.SendAsync(
				"ReceiveNextRound",
				roundNumber,
				numberOfPlayersToAdvance,
				secondsRemaining,
				problem);

			Lobby.SetTimer(secondsRemaining);
		}
	}
}
