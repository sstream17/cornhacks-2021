using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoderRoyale
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			var processInfo = new ProcessStartInfo()
			{
				FileName = "docker",
				Arguments = $"build --tag docker-code {currentDirectory}\\DockerRuntime",
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

			process.Start();
			process.BeginOutputReadLine();
			process.WaitForExit();
			process.Close();

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
