using Discord;
using Discord.Commands;
using System;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;

namespace TheGoodBot.Core.Services
{
    public class CommandSucceededService
    {
        private LoggerService _logger;
        private InvokeService _invoke;

        public CommandSucceededService(LoggerService logger, InvokeService invoke)
        {
            _logger = logger;
            _invoke = invoke;
        }

        /// <summary> Checks the command result if succeeded and behaves accordingly.</summary>
        /// <param name="result"></param>
        public void SucceededCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            LogMessage((SocketCommandContext) context, result);
            CheckForInvocation(command, context);
        }

        private void LogMessage(ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | Command failed | User: {context.User.Username}/{context.User.Id} | ";
            var message = $"{result.ErrorReason}";
            _logger.LogSucceededCommand($"\r\n{prefix}-{message}", context.Guild.Id);
        }

        private void CheckForInvocation(Optional<CommandInfo> command, ICommandContext context)
        {
            _invoke.InvokeCommand($"{command.Value.Module.Group}-{command.Value.Name}", context.Guild.Id);
        }
    }
}