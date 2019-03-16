using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services
{
    public class CommandSucceededService
    {
        private LoggerService _logger;
        private GuildAccountService _guildAccount;

        public CommandSucceededService(LoggerService logger, GuildAccountService guildAccount)
        {
            _logger = logger;
            _guildAccount = guildAccount;
        }

        /// <summary> Checks the command result if succeeded and behaves accordingly.</summary>
        /// <param name="result"></param>
        public void SucceededCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            LogMessage((SocketCommandContext) context, result);
            CheckForInvocation(command, context, context.Message);
        }

        private void LogMessage(ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | Command failed | User: {context.User.Username}/{context.User.Id} | ";
            var message = $"{result.ErrorReason}";
            _logger.LogSucceededCommand($"\r\n{prefix}-{message}", context.Guild.Id);
        }

        private void CheckForInvocation(Optional<CommandInfo> command, ICommandContext context, IUserMessage message)
        {
            var invokeTime = _guildAccount.GetInvocation($"{command.Value.Module.Group}-{command.Value.Name}", context.Guild.Id);
            if (invokeTime != 0)
            {
                Task.Delay(invokeTime).ContinueWith(t => message.DeleteAsync()); 
            }

            return;
        }
    }
}