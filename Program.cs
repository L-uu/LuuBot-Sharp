using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using Newtonsoft.Json;

namespace LuuBot_Sharp
{
	class Program
	{
		static void Main(string[] args)
		{
			MainAsync().GetAwaiter().GetResult();
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
			await discord.ConnectAsync();
			await Task.Delay(-1);
		}
	}
}
