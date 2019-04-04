﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TheGoodBot.Core.Preconditions
{
    public class RequireHigherHierarchy : ParameterPreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, ParameterInfo parameter,
            object value, IServiceProvider service)
        {
            if (!(value is SocketGuildUser user))
            {
                return Task.FromResult(PreconditionResult.FromError("You must mention a valid user."));
            }

            var bot = (SocketGuildUser)context.Client.CurrentUser;
            if (bot.Guild.Roles.OrderByDescending(r => r.Position).FirstOrDefault().Position <= user.Hierarchy)
            {
                return Task.FromResult(PreconditionResult.FromError("The bot must be ranked higher than the mentioned user."));
            }

            return ((context.Guild as SocketGuild).CurrentUser.Hierarchy > user.Hierarchy)
                    ? Task.FromResult(PreconditionResult.FromSuccess())
                    : Task.FromResult(PreconditionResult.FromError("You must be ranked higher than the mentioned user."));
        }
    }
}
