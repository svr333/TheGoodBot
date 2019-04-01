using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Services.Languages;

namespace TheGoodBot.Core.Services
{
    public class CommandFailedService
    {
        private LoggerService _logger;
        private CustomEmbedService _customEmbed;

        public CommandFailedService(CustomEmbedService customEmbed, LoggerService logger)
        {
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

            else if (result.ErrorReason.StartsWith("You do not have the required permission"))
            {
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext) context, "NoBotOwner");
            }

            else if (result.ErrorReason == "This command may only be invoked in an NSFW channel.")
            {
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext) context, "RequireNSFW");
            }

            else
            {
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext) context, "UncalculatedError");
            }
        }

        private void LogMessage(ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | Command failed | User: {context.User.Username}/{context.User.Id} | ";
            string suffix = $"";
            var message = $"{result.ErrorReason}";
            _logger.LogFailedCommand($"\r\n{prefix}-{message}-{suffix}", context.Guild.Id);
        }
    }
}