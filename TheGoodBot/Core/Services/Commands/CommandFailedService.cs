using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Services.Languages;
using TheGoodBot.Core.Services.Logging;

namespace TheGoodBot.Core.Services
{
    public class CommandFailedService
    {
        private LoggerService _logger;
        private FailedCommandLogService _fileLog;
        private CustomEmbedService _customEmbed;

        public CommandFailedService(FailedCommandLogService fileLog, CustomEmbedService customEmbed, LoggerService logger)
        {
            _fileLog = fileLog;
            _customEmbed = customEmbed;
            _logger = logger;
        }

        /// <summary> Checks the command result if failed and behaves accordingly.</summary>
        /// <param name="result"></param>
        public async Task FailedCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            LogMessage(context, result);
            Console.WriteLine("Successfully logged failed command.");

            if (result.ErrorReason.StartsWith("You can use this command in"))
            {
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext) context, "CommandOnCooldown"); 
            }

            if (result.ErrorReason.StartsWith("You do not have the required permission"))
            {
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext) context, "NoBotOwner");
            }

            if (result.ErrorReason.StartsWith("UnmetPrecondition: "))
            {
                
            }
        }

        public void LogMessage(ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | Command failed | User: {context.User.Username}/{context.User.Id} | ";
            var message = $"{result.ErrorReason}";
            _logger.LogFailedCommand($"\r\n{prefix}-{message}", context.Guild.Id);
        }
    }
}