using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Entities;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services
{
    public class EventHookerService
    {
        private DiscordSocketClient _client;
        private GuildAccountService _guildAccount;
        private CreateLanguageFilesService _language;
        private BotConfig _config;

        public EventHookerService(DiscordSocketClient client, CreateLanguageFilesService language, GuildAccountService guildAccount, 
            BotConfig config)
        {
            _client = client;
            _language = language;
            _guildAccount = guildAccount;
            _config = config;
        }

        public void HookEvents()
        {
            _client.Ready += Ready;
            _client.JoinedGuild += GuildJoined;
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