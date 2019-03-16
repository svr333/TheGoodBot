using Discord.Commands;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Preconditions
{
    public class Invoke : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            var _guildAccount = services.GetRequiredService<GuildAccountService>();

            var sGuildAccount = _guildAccount.GetSettingsAccount(context.Guild.Id);
            var invokeTime = _guildAccount.GetInvocation($"{command.Module.Group}-{command.Name}", context.Guild.Id);

            Task.Delay((int) invokeTime);
            if (invokeTime != 0)
            {
                context.Message.DeleteAsync();
            }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}