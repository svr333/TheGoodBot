using System;
using Discord;
using Discord.Commands;

namespace TheGoodBot.Core.Services
{
    public class CommandFailedService
    {
        /// <summary> Checks the command result if failed and behaves accordingly.</summary>
        /// <param name="result"></param>
        public void FailedCommandResult(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (result.ErrorReason.StartsWith("You can use this command in"))
            {
                Console.WriteLine("I'm right here where you want me to be");
            }
        }
    }
}