using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Services;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;

namespace TheGoodBot.Core.Preconditions
{
    public class Cooldown : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var guildAccountService = services.GetRequiredService<GuildAccountService>();
            var cooldownService = services.GetRequiredService<GlobalUserCooldowns>();

            var maxCooldown = guildAccountService.GetMaxCooldown($"{command.Module.Group}-{command.Name}", context.Guild.Id);
            var sGuildAccount = guildAccountService.GetSettingsAccount(context.Guild.Id);
            var allowedUsersAndRoles = sGuildAccount.AllowedUsersAndRolesToBypassCooldowns;
            var ts = TimeSpan.FromMilliseconds(maxCooldown);

            if (sGuildAccount.AllowAdminsToBypassCooldowns && context.User is IGuildUser user 
                     && user.GuildPermissions.Administrator || allowedUsersAndRoles.ValidatePermissions(context))
            return Task.FromResult(PreconditionResult.FromSuccess());

            var key = $"{command.GetHashCode()}-{context.User.Id}";
            if (cooldownService.GetUserCooldown(key, out DateTime endsAt))
            {
                var difference = endsAt.Subtract(DateTime.UtcNow);
                if (difference.Seconds > 0)
                {
                    Console.WriteLine($"You can use this command in {difference.ToString(@"mm\m\:ss\s")}");
                    return Task.FromResult(PreconditionResult.FromError(
                        $"You can use this command in {difference.ToString(@"mm\m\:ss\s")}"));
                }

                var time = DateTime.UtcNow.Add(ts);
                cooldownService.ChangeUserCooldown(key, time, endsAt);
            }
            else { cooldownService.AddUserCooldown(key, DateTime.UtcNow.Add(ts)); }

            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
