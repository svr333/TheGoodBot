using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Core.Services.Languages;

namespace TheGoodBot.Core.Services
{
    public class CommandHandlerService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly GuildAccountService _guildAccount;
        private readonly EventHookerService _eventHooker;
        private readonly CustomEmbedService _customEmbed;
        private readonly CommandFailedService _commandFailed;
        private CommandSucceededService _commandSucceeded;

        public CommandHandlerService(IServiceProvider services, DiscordSocketClient client, CommandService commands, 
            GuildAccountService guildAccount, EventHookerService eventHooker, CustomEmbedService customEmbed,
            CommandFailedService commandFailed, CommandSucceededService commandSucceeded)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _guildAccount = guildAccount;
            _eventHooker = eventHooker;
            _customEmbed = customEmbed;
            _commandFailed = commandFailed;
            _commandSucceeded = commandSucceeded;
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

            var user = message.Author as SocketGuildUser;

            var guild = _guildAccount.GetSettingsAccount(user.Guild.Id);

            if (!(message.HasPrefix(_client, out int argPos, guild.PrefixList))) { return; }
            if (!guild.BotsCanInteract && message.Author.IsBot) { return; }

            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            var guildAccount = _guildAccount.GetSettingsAccount(context.Guild.Id);
            if (!command.IsSpecified)
            {
                if (guildAccount.NoCommandFoundResponseIsDisabled) { return; }

                await _customEmbed.CreateAndPostEmbeds((SocketCommandContext)context, "NoCommandFound");
                return;
            }

            if (result.IsSuccess)
            {
                _commandSucceeded.SucceededCommandResult(command, context, result);
                Console.WriteLine($"Command executed: {context.User.Username} used {command.Value.Name}");
                return;
            }
            else
            {
                await _commandFailed.FailedCommandResult(command, context, result);
            }
        }
    }
}
