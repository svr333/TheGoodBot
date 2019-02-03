using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using TheGoodBot.Entities;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Preconditions
{
    public class RequireBotOwner : PreconditionAttribute
    {
        private ulong _botOwnerId;

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            var configService = services.GetRequiredService<BotConfigService>();
            _botOwnerId = configService.GetConfig().BotOwnerID;
            if (context.User.Id == _botOwnerId)
            {
                  return PreconditionResult.FromSuccess();
            }
            else return PreconditionResult.FromError($"You do not have the required permission to use this command.");
        }
    }
}