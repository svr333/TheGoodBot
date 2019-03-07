using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace TheGoodBot.Core.Preconditions
{
    public class Invoke : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}