using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services
{
    public class EventHookerService
    {
        private readonly DiscordSocketClient _client;
        private readonly GuildAccountService _guildAccount;
        private readonly CreateLanguageFilesService _language;
        private readonly BotConfig _config;
        private readonly GuildLogsService _guildLogs;

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
            // TODO: Filter out ?purge command - I have no clue how though
            var logChannel = (_client.GetChannel(_guildLogs.GetGuildLogs(((SocketTextChannel)channel).Guild.Id).MessageDeletedChannelId)) as SocketTextChannel;
            if (logChannel.Id == channel.Id) { return; }
            if (logChannel.Guild != ((SocketTextChannel) channel).Guild)
            {
                await channel.SendMessageAsync("The log channel for deleted messages must be in this server, please change it.");
                return;
            }
            // TODO: Check if it was command && if the guild disabled logging commands
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

            if (string.IsNullOrEmpty(cachedMessage.Value.Content) && cachedMessage.Value.Embeds.Count != 0)
            {
                embed.AddField($"Message solely contained embeds.", $"Cannot show content of embeds.");
                // TODO: save the raw embed json somewhere & make a command that'll show the embed.
                // Or something better of course
            }
            else if (string.IsNullOrEmpty(cachedMessage.Value.Content))
            {
                embed.AddField("Message contents:" , $"{cachedMessage.Value.Content}");
            }
            else
            {
                embed.AddField($"No text found", $"Must have been a file then...");
            }

            await logChannel.SendMessageAsync(embed: embed.Build());
        }

        private async Task GuildJoined(SocketGuild guild)
        {
            _guildAccount.AddGuild(guild.Id);
            await Task.CompletedTask;
        }

        private async Task Ready()
        {
            await CreateAllFilesAsync();
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync(_config.GameStatus);
            Console.WriteLine("Ready, sir.");
        }

        private async Task CreateAllFilesAsync()
        {
            _language.CreateAllLanguageFiles();
            _guildAccount.CreateAllGuildAccounts();
            _guildAccount.CreateAllGuildCooldownsAndInvocations();

        }
    }
}
