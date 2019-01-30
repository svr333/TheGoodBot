using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheGoodBot.Core.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly Logger _logger;

        /// <summary>
        /// This allows us to get everything we need from DI when the class is instantiated.
        /// </summary>
        /// <param name="services">Get The Services from DI.</param>
        /// <param name="client">Get The Client from DI.</param>
        /// <param name="cmdService">Get The CommandService from DI.</param>
        public CommandHandlerService(IServiceProvider services, DiscordSocketClient client, CommandService commands, Logger logger)
        {
            //Set everything we need from DI.
            _commands = commands;
            _client = client;
            _services = services;
            _logger = logger;
        }

        //This is the task we will call to create out command service.
        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            //Just like in the client, here we hook any command specific events.
            HookEvents();
        }

        private void HookEvents()
        {
            _client.MessageReceived += HandlerMessageAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
            _commands.Log += LogAsync;
        }

        private Task LogAsync(LogMessage message)
        {
            _logger.Log(message.Message);
            return Task.CompletedTask;
        }

        private async Task HandlerMessageAsync(SocketMessage socketMessage)
        {
            // Don't process the command if it was a system message
            if (!(socketMessage is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;



            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.

            // Keep in mind that result does not indicate a return value
            // rather an object stating if the command executed successfully.
            var result = await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }


        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // The command failed because it isn't a command found in out bot. (Ignore it)
            if (!command.IsSpecified)
            {
                Console.WriteLine(result);
                return;
            }


            // The Command Worked, Log to console, who used it and what the command was.
            if (result.IsSuccess)
            {
                Console.WriteLine($"Command: {context.User.Username} used {command.Value.Name}");
                return;
            }


            /* the command failed, let's notify the user that something happened. */
            Console.WriteLine($"COMMAND ERROR: {result}");
            await context.Channel.SendMessageAsync($"error: {result}");
        }

    }
}