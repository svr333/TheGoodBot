using TheGoodBot.Core.Services;
using TheGoodBot.Entities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Core.Services.Logging;
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
        private BotConfig _config;
        private LoggerService _logger;

        public BasicBotClient(CommandService commands = null, DiscordSocketClient client = null, BotConfig config = null,
            LoggerService logger = null)
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
            _logger = logger ?? new LoggerService();
        }


        public async Task InitializeAsync()
        {
            _services = ConfigureServices();

            await _client.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await _client.StartAsync();



            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();
            await Task.Delay(-1);
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<BotConfigService>()
                .AddSingleton<LoggerService>()
                .AddSingleton<GuildAccountService>()
                .AddSingleton<GuildUserAccountService>()
                .AddSingleton<GlobalUserAccountService>()
                .AddSingleton<CreateLanguageFilesService>()
                .AddSingleton<LanguageService>()
                .AddSingleton<CustomEmbedService>()
                .AddSingleton<EventHookerService>()
                .AddSingleton<CreateGuildAccountFilesService>()
                .AddSingleton<GuildFilesGenerationService>()
                .AddSingleton<InteractiveService>()
                .AddSingleton<CooldownService>()
                .AddSingleton<BotConfig>()
                .AddSingleton<CommandFailedService>()
                .AddSingleton<FailedCommandLogService>()
                .AddSingleton<SucceededCommandLogService>()
                .AddSingleton<LoggerService>()
                .AddSingleton<FailedCommandLogService>()
                .BuildServiceProvider();
        }
    }
}
