using System.Collections.Generic;
using System.Timers;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoderRoyale.Hubs
{
	public static class Lobby
	{
		public static bool inLobby = true;
		public static Dictionary<string, string> Players = new Dictionary<string, string>();

		private static Timer timer;

		private static HubConnection connection = new HubConnectionBuilder()
				.WithUrl("https://localhost:44316/gamehub")
				.Build();

		public async static void SetTimer(int seconds)
		{
			if (!connection.State.Equals(HubConnectionState.Connected))
			{
				await connection.StartAsync();
			}

			timer = new Timer(seconds * 1000);
			timer.Elapsed += OnTimer;
			timer.Enabled = true;
		}

		private async static void OnTimer(object source, ElapsedEventArgs e)
		{
			await connection.InvokeAsync("SendEndGame");
		}
	}
}
