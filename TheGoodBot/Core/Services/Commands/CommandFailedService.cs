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
        private FileLoggerService _fileLog;
        private CustomEmbedService _customEmbed;

        public CommandFailedService(FileLoggerService fileLog, CustomEmbedService customEmbed)
        {
            _fileLog = fileLog;
            _customEmbed = customEmbed;
        }

        /// <summary> Checks the command result if failed and behaves accordingly.</summary>
        /// <param name="result"></param>
        public async Task FailedCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.ErrorReason.StartsWith("You can use this command in"))
            {
                Console.WriteLine("Command failed because it's on a cooldown.");
                await _customEmbed.CreateAndPostEmbed((SocketCommandContext)context, "CommandOnCooldown");
            }
        }
    }
}