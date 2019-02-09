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
    public class EventHooker
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private Logger _logger;
        private GuildAccountService _guild;
        private GuildUserAccountService _guildUser;
        private GlobalUserAccountService _user;
        private CreateLanguageFilesService _language;

        public EventHooker(DiscordSocketClient client, CommandService command, Logger logger, GuildAccountService guild,
            GuildUserAccountService guildUser, GlobalUserAccountService user, CreateLanguageFilesService language)
        {
            _client = client;
            _commands = command;
            _logger = logger;
            _guild = guild;
            _guildUser = guildUser;
            _user = user;
            _language = language;
        }

        public void HookEvents()
        {
            _commands.Log += LogAsync;
            _client.Ready += Ready;
        }

        private Task Ready()
        {
            _language.CreateAllLanguageFiles();
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage message)
        {
            _logger.Log(message.Message);
            return Task.CompletedTask;
        }
    }
}