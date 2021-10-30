using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using LuuBot_Sharp.Commands;
using System.IO;
using System.Threading.Tasks;

namespace LuuBot_Sharp
{
	class Program
	{
		static void Main(string[] args)
		{
			MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
		}

		static async Task MainAsync()
		{
			string token = File.ReadAllText("token.txt");
			var discord = new DiscordClient(new DiscordConfiguration()
			{
				Token = token,
				TokenType = TokenType.Bot,
				Intents = DiscordIntents.AllUnprivileged
			});
			var commands = discord.UseCommandsNext(new CommandsNextConfiguration()
			{
				StringPrefixes = new[] { "-" }
			});
			commands.RegisterCommands<MusicCommands>();

			var endpoint = new ConnectionEndpoint
			{
				Hostname = "127.0.0.1",
				Port = 2333
			};
			var lavalinkConfig = new LavalinkConfiguration
			{
				Password = "youshallnotpass",
				RestEndpoint = endpoint,
				SocketEndpoint = endpoint
			};
			var lavalink = discord.UseLavalink();

			await discord.ConnectAsync();
			await lavalink.ConnectAsync(lavalinkConfig);
			await Task.Delay(-1);
		}
	}
}
