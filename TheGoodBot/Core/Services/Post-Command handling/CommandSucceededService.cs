using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Core.Services.Logging;

namespace TheGoodBot.Core.Services
{
    public class CommandSucceededService
    {
        private LoggerService _logger;
        private GuildAccountService _guildAccount;
        private bool _commandInvokes;

        public CommandSucceededService(LoggerService logger, GuildAccountService guildAccount)
        {
            _logger = logger;
            _guildAccount = guildAccount;
        }

        /// <summary> Checks the command result if succeeded and behaves accordingly.</summary>
        /// <param name="result"></param>
        public void SucceededCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            _commandInvokes = CheckForInvocation(command, context, context.Message);
            LogMessage((SocketCommandContext)context, result);
            Console.WriteLine("Successfully logged succeeded command.");
        }

        private void LogMessage(ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | Command succeeded | User: {context.User.Username}/{context.User.Id} | ";
            string suffix = $"Invocation: {_commandInvokes}";
            var message = $"{result.ErrorReason}";
            _logger.LogSucceededCommand($"\r\n{prefix}-{message}-{suffix}", context.Guild.Id);
        }

        private bool CheckForInvocation(Optional<CommandInfo> command, ICommandContext context, IUserMessage message)
        {
            var invokeTime = _guildAccount.GetInvocation($"{command.Value.Module.Group}-{command.Value.Name}", context.Guild.Id);
            if (invokeTime != 0)
            {
                Task.Delay(invokeTime).ContinueWith(t => message.DeleteAsync());
                return true;
            }

            return false;
        }
    }
}
