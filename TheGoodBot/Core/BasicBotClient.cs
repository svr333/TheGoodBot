using TheGoodBot.Core.Services;
using TheGoodBot.Entities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core
{
    public class BasicBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private BotConfigStruct _config;
        private Logger _logger;

        public BasicBotClient(CommandService commands = null, DiscordSocketClient client = null, BotConfigStruct config = null, Logger logger = null)
        {
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100
            });

            _commands = commands ?? new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            });

            _config = config ?? new BotConfigService().GetConfig();
            _logger = logger ?? new Logger();
        }


        public async Task InitializeAsync()
        {
            _services = ConfigureServices();

            await _client.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await _client.StartAsync();

            HookEvents();

            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();
            await Task.Delay(-1);
        }

        private void HookEvents()
        {
            _client.Log += LogAsync;
            _client.Ready += OnReadyAsync;
        }

        private async Task OnReadyAsync()
        {
            await _client.SetGameAsync(_config.GameStatus);
        }

        private Task LogAsync(LogMessage log)
        {
            _logger.Log(log.Message);
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<BotConfigService>()
                .AddSingleton<Logger>()
                .AddSingleton<GuildAccountService>()
                .AddSingleton<GuildUserAccountService>()
                .AddSingleton<GlobalUserAccountService>()
                .AddSingleton<CreateLanguageFilesService>()
                .AddSingleton<LanguageService>()
                .AddSingleton<CustomEmbedService>()
                .AddSingleton<EventHooker>()
                .BuildServiceProvider();
        }
    }
}
