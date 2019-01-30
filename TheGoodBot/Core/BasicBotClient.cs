using TheGoodBot.Core.Services;
using TheGoodBot.Entities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core
{
    public class BasicBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private BotConfig _config;
        private Logger _logger;

        //Initialize The Client And Config.
        public BasicBotClient(CommandService commands = null, DiscordSocketClient client = null, BotConfig config = null, Logger logger = null)
        {
            //Create our new DiscordClient (Setting LogServerity to Verbose)
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100
            });

            //Create our new CommandService (Setting RunMode to async by default on all commands)
            _commands = commands ?? new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false,
                LogLevel = LogSeverity.Verbose
            });

            //Get our config data from DataStorage. (config.json)
            _config = config ?? new BotConfigDataHandler().GetConfig();
            _logger = logger ?? new Logger();
        }


        public async Task InitializeAsync()
        {
            _services = ConfigureServices();

            await _client.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await _client.StartAsync();

            HookEvents();

            //Initialize Our CommandService Handler
            await _services.GetRequiredService<CommandHandlerService>().InitializeAsync();
            await Task.Delay(-1);
        }

        //This is where we hook up any events we want to use.
        private void HookEvents()
        {
            _client.Log += LogAsync;
            _client.Ready += OnReadyAsync;
        }

        //When the client sends the event, indicating that it is ready, we set the Now Playing game to what is in our config.json
        private async Task OnReadyAsync()
        {
            await _client.SetGameAsync(_config.GameStatus);
        }

        private Task LogAsync(LogMessage log)
        {
            //Display any log messages to the console.
            _logger.Log(log.Message);
            return Task.CompletedTask;
        }


        //This is used to add any Services to the DI Service Provider.
        //These services are then injected wherever they're needed.
        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<BotConfigDataHandler>()
                .AddSingleton<Logger>()
                .BuildServiceProvider();
        }
    }
}
