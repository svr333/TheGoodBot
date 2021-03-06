﻿using System;
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
            LogMessage(command, context, result);

            if (result.ErrorReason == "CommandOnCooldown")
            {
                await _customEmbed.CreateAndPostEmbeds((SocketCommandContext) context, "CommandOnCooldown");
            }

            else if (result.ErrorReason == "This command may only be invoked in an NSFW channel.")
            {
                await _customEmbed.CreateAndPostEmbeds((SocketCommandContext) context, "RequireNSFW");
            }
            else if (result.ErrorReason == "NegativeValueInput")
            {
                await context.Channel.SendMessageAsync("success");
            }

            else
            {
                await _customEmbed.CreateAndPostEmbeds((SocketCommandContext) context, "UncalculatedError");
            }
        }

        private void LogMessage(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            string prefix = $"{DateTime.Now} | {command.Value.Name}";
            string suffix = $" User: {context.User.Username}/{context.User.Id}";
            var message = $"{result.ErrorReason}";
            _logger.LogFailedCommand($"\r\n{prefix}-{message}-{suffix}", context.Guild.Id);
        }
    }
}
