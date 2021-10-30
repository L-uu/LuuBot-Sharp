using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using System.Linq;
using System.Threading.Tasks;

namespace LuuBot_Sharp.Commands
{
	class MusicCommands : BaseCommandModule
	{
		[Command("join")]
		public async Task JoinCommand(CommandContext ctx)
		{
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			if (ctx.Member.VoiceState == null)
			{
				await ctx.RespondAsync("You must be in a voice channel to do this.");
				return;
			}
			DiscordChannel channel = ctx.Member.VoiceState.Channel;
			await node.ConnectAsync(channel);
			await ctx.RespondAsync($"Successfully joined **{channel.Name}**");
		}

		[Command("leave")]
		public async Task LeaveCommand(CommandContext ctx)
		{
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Channel.Guild);
			if (conn == null)
			{
				await ctx.RespondAsync("I am not connected to a voice channel.");
				return;
			}
			await conn.DisconnectAsync();
			await ctx.RespondAsync($"Successfully disconnected from **{conn.Channel.Name}**");
		}

		[Command("play")]
		public async Task PlayCommand(CommandContext ctx, [RemainingText] string search)
		{
			if (ctx.Member.VoiceState == null)
			{
				await ctx.RespondAsync("You are not connected to a voice channel.");
				return;
			}
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			if (conn == null)
			{
				await node.ConnectAsync(ctx.Member.VoiceState.Channel);
				conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			}
			var searchResults = await node.Rest.GetTracksAsync(search);
			if (searchResults.LoadResultType == LavalinkLoadResultType.LoadFailed
				|| searchResults.LoadResultType == LavalinkLoadResultType.NoMatches)
			{
				await ctx.RespondAsync($"Track search failed for **{search}**");
				return;
			}
			var track = searchResults.Tracks.First();
			await conn.PlayAsync(track);
			await ctx.RespondAsync($"Now playing: **{track.Title}**");
		}

		[Command("stop")]
		public async Task StopCommand(CommandContext ctx)
		{
			var lava = ctx.Client.GetLavalink();
			var node = lava.ConnectedNodes.Values.First();
			var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
			await conn.StopAsync();
			await conn.DisconnectAsync();
		}
	}
}
