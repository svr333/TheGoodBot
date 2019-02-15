using Discord;
using Discord.Commands;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TheGoodBot.Core.Extensions;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Preconditions
{
    public class Cooldown : PreconditionAttribute
    {
        readonly ConcurrentDictionary<CooldownInfo, DateTime> _cooldowns = new ConcurrentDictionary<CooldownInfo, DateTime>();

        public struct CooldownInfo
        {
            public ulong UserId { get; }
            public int CommandHashCode { get; }

            public CooldownInfo(ulong userId, int commandHashCode)
            {
                UserId = userId;
                CommandHashCode = commandHashCode;
            }
        }

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            var guildAccountService = services.GetRequiredService<GuildAccountService>();
            var cooldown = guildAccountService.GetCooldown($"{command.Module.Group}-{command.Name}", context.Guild.Id);
            var Sguild = guildAccountService.GetSettingsAccount(context.Guild.Id);
            var ts = TimeSpan.FromSeconds(cooldown);

            if (!Sguild.AdminsAreLimited && context.User is IGuildUser user && user.GuildPermissions.Administrator || UserIsAllowedToBypass())
                return Task.FromResult(PreconditionResult.FromSuccess());


            var key = new CooldownInfo(context.User.Id, command.GetHashCode());
            if (_cooldowns.TryGetValue(key, out DateTime endsAt))
            {
                var difference = endsAt.Subtract(DateTime.UtcNow);
                if (difference.Ticks > 0)
                {
                    return Task.FromResult(PreconditionResult.FromError($"You can use this command in {difference.ToString(@"mm\:ss")}"));
                }

                var time = DateTime.UtcNow.Add(ts);
                _cooldowns.TryUpdate(key, time, endsAt);
            }
            else
            {
                _cooldowns.TryAdd(key, DateTime.UtcNow.Add(ts));
            }

            return Task.FromResult(PreconditionResult.FromSuccess());

            bool UserIsAllowedToBypass()
            {
                var guildUser = (IGuildUser) context.User;
                var roleOrUserID = Sguild.AllowedUsersAndRolesToBypassCooldowns;

                for (int i = 0; i < Sguild.AllowedUsersAndRolesToBypassCooldowns.Count; i++)
                {
                    if (context.User.Id == roleOrUserID[i] || guildUser.UserHasRole(roleOrUserID[i])) { return true; }
                }
                return false;
            }
        }
    }
}