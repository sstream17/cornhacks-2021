using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace CoderRoyale.Hubs
{
	public class GameHub : Hub
	{
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
			await Clients.All.SendAsync(
				"ReceiveNextRound",
				roundNumber,
				numberOfPlayersToAdvance);
		}
	}
}
