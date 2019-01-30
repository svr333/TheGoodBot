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

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
        }


        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            //Make this toggleable
            if (!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync($"This command is not defined.");
                return;
            }

            if (result.IsSuccess)
            {
                //save this to a persistent data file + track on the userprofiles
                Console.WriteLine($"Command executed: {context.User.Username} used {command.Value.Name}");
                return;
            }


            /* the command failed, let's notify the user that something happened. */
            Console.WriteLine($"COMMAND ERROR: {result}");
            await context.Channel.SendMessageAsync($"There was an 'uncalculated' error executing the command: {result}\n Contact svr333 / <@202095042372829184> for more information.");
        }

    }
}