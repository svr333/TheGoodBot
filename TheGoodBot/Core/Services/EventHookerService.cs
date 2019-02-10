using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheGoodBot.Core.Services.Accounts;
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
        private GuildUserAccountService _guildUser;
        private GlobalUserAccountService _user;
        private CreateLanguageFilesService _language;
        private GuildListService _guildList;

        public EventHookerService(DiscordSocketClient client, CommandService command, LoggerService logger,
            GuildUserAccountService guildUser, GlobalUserAccountService user, CreateLanguageFilesService language,
            GuildListService guildList)
        {
            _client = client;
            _commands = command;
            _logger = logger;
            _guildUser = guildUser;
            _user = user;
            _language = language;
            _guildList = guildList;
        }

        public void HookEvents()
        {
            _commands.Log += LogAsync;
            _client.Ready += Ready;
            _client.JoinedGuild += GuildJoined;
        }

        private Task GuildJoined(SocketGuild guild)
        {
            _guildList.AddGuild(guild.Id);
            return Task.CompletedTask;
        }

        private Task Ready()
        {
            _language.CreateAllLanguageFiles();
            _guildList.CreateAllGuildAccounts();
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage message)
        {
            _logger.Log(message.Message);
            return Task.CompletedTask;
        }
    }
}