using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly GuildAccountService _guildAccountService;
        private readonly EventHooker _eventHooker;

        public CommandHandlerService(IServiceProvider services, DiscordSocketClient client, CommandService commands, 
            GuildAccountService guildAccount, EventHooker eventHooker)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _guildAccountService = guildAccount;
            _eventHooker = eventHooker;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            HookCommandHandlers();
            _eventHooker.HookEvents();
        }

        private void HookCommandHandlers()
        {
            _client.MessageReceived += HandlerMessageAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
        }

        public async Task HandlerMessageAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)) return;

            if (message.Channel is IPrivateChannel)
            {
                await message.Author.SendMessageAsync($"Sorry, I only accept messages in a guild.");
                return;
            }

            int argPos;
            var guildUser = message.Author as SocketGuildUser;
            var guildID = guildUser.Guild.Id;
            var guild = _guildAccountService.GetOrCreateGuildAccount(guildID);

            if (!(PrefixCheckerExt.HasPrefix(message, _client, out argPos, guild.PrefixList))) { return; }
            if (message.Author.IsBot) { return; }

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            Console.WriteLine(result);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            var guildID = context.Guild.Id;
            var guildAccount = _guildAccountService.GetOrCreateGuildAccount(guildID);
            if (guildAccount.NoCommandFoundIsDisabled) { return; }
            if (!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync($"This command is not defined.");
                return;
            }

            if (result.IsSuccess)
            {
                // Todo: save this to a persistent data file + track on the userprofiles
                Console.WriteLine($"Command executed: {context.User.Username} used {command.Value.Name}");
                return;
            }

            await context.Channel.SendMessageAsync($"There was an 'uncalculated' error executing the command: {result}\nContact svr333 / <@202095042372829184> for more information.");
        }
    }
}