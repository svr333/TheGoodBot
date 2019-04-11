using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Entities;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;

namespace TheGoodBot.Core.Services
{
    public class EventHookerService
    {
        private DiscordSocketClient _client;
        private GuildAccountService _guildAccount;
        private CreateLanguageFilesService _language;
        private BotConfig _config;
        private GuildLogsService _guildLogs;

        public EventHookerService(DiscordSocketClient client, CreateLanguageFilesService language, GuildAccountService guildAccount, 
            BotConfig config, GuildLogsService guildLogs)
        {
            _client = client;
            _language = language;
            _guildAccount = guildAccount;
            _config = config;
            _guildLogs = guildLogs;
        }

        public void HookEvents()
        {
            _client.Ready += Ready;
            _client.JoinedGuild += GuildJoined;
            _client.MessageDeleted += LogDeletedMessage;
        }

        private async Task LogDeletedMessage(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
        {
            var logChannel = (_client.GetChannel(_guildLogs.GetGuildLogs(((SocketTextChannel)channel).Guild.Id).MessageDeletedChannelId)) as SocketTextChannel;

            // TODO: Check if it was command && if the guild disabled logging commands
            if (channel is null) { await Task.CompletedTask; }
            if (cachedMessage.Value is null) { return; }

            var auditLogs = await _client.Rest.GetGuildAsync(logChannel.Guild.Id).Result.GetAuditLogsAsync(1).FlattenAsync();
            var moderator = auditLogs.FirstOrDefault(x => x.Action == ActionType.MessageDeleted);

            var embed = new EmbedBuilder()
                .WithAuthor(cachedMessage.Value.Author)
                .WithColor(((SocketGuildUser)cachedMessage.Value.Author).GetUserTopColour())
                .WithTitle($"Message deleted in #{channel.Name}")
                .WithFooter($"UserId: {cachedMessage.Value.Author.Id}")
                .WithCurrentTimestamp();

            if (moderator is null || (DateTime.Now - moderator.CreatedAt.DateTime).TotalMilliseconds > 750 ) { embed.WithDescription($"Message deleted by a bot or the user self."); }
            else { embed.WithDescription($"Message deleted by {moderator.User}."); }

            if (cachedMessage.Value.Content is null || cachedMessage.Value.Content is "" && cachedMessage.Value.Embeds.Count != 0)
            {
                embed.AddField($"Message solely contained embeds.", $"Cannot show content of embeds.");
                // TODO: save the raw embed json somewhere & make a command that'll show the embed.
                // Or something better of course
            }
            else if (cachedMessage.Value.Content != null && cachedMessage.Value.Content != "")
            {
                embed.AddField("Message contents:" , $"{cachedMessage.Value.Content}");
            }
            else
            {
                embed.AddField($"No text found", $"Must've been a file then...");
            }

            await logChannel.SendMessageAsync(embed: embed.Build());
        }

        private async Task GuildJoined(SocketGuild guild)
        {
            _guildAccount.AddGuild(guild.Id);
            await Task.CompletedTask;
        }

        private Task Ready()
        {
            _language.CreateAllLanguageFiles();
            _guildAccount.CreateAllGuildAccounts();
            _guildAccount.CreateAllGuildCooldownsAndInvocations();
            _client.SetStatusAsync(UserStatus.DoNotDisturb);
            _client.SetGameAsync(_config.GameStatus);
            Console.WriteLine("Ready, sir.");
            return Task.CompletedTask;
        }
    }
}