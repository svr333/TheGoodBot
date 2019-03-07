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
        private CommandService _commands;
        private LoggerService _logger;
        private GuildAccountService _guildAccount;
        private GuildUserAccountService _guildUser;
        private GlobalUserAccountService _user;
        private CreateLanguageFilesService _language;
        private CooldownService _cooldown;
        private BotConfig _config;

        public EventHookerService(DiscordSocketClient client, CommandService command, LoggerService logger,
            GuildUserAccountService guildUser, GlobalUserAccountService user, CreateLanguageFilesService language,
            GuildAccountService guildAccount, CooldownService cooldown, BotConfig config)
        {
            _client = client;
            _commands = command;
            _logger = logger;
            _guildUser = guildUser;
            _user = user;
            _language = language;
            _guildAccount = guildAccount;
            _cooldown = cooldown;
            _config = config;
        }

        public void HookEvents()
        {
            _commands.Log += LogAsync;
            _client.Ready += Ready;
            _client.JoinedGuild += GuildJoined;
        }

        private async Task GuildJoined(SocketGuild guild)
        {
            _guildAccount.AddGuild(guild.Id);
            await Task.CompletedTask;
        }

        private async Task Ready()
        {
            _language.CreateAllLanguageFiles();
            _guildAccount.CreateAllGuildAccounts();
            _guildAccount.CreateAllGuildCooldowns();
            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetGameAsync(_config.GameStatus);
        }

        private async Task LogAsync(LogMessage message)
        {
            _logger.Log(message.Message);
            await Task.CompletedTask;
        }
    }
}