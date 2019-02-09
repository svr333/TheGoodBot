using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace TheGoodBot.Core.Services
{
    public class EventHooker
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private Logger _logger;

        public EventHooker(DiscordSocketClient client, CommandService command, Logger logger)
        {
            _client = client;
            _commands = command;
            _logger = logger;
        }


        public void HookEvents()
        {
            _commands.Log += LogAsync;
            _client.Ready += Ready;
        }

        private Task Ready()
        {
            Console.WriteLine($"I'm ready, sir.");
            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage message)
        {
            _logger.Log(message.Message);
            return Task.CompletedTask;
        }
    }
}