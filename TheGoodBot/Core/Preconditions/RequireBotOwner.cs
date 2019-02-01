using System;
using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Entities;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Preconditions
{
    public class RequireBotOwner : PreconditionAttribute
    {
        private readonly ulong _userId;
        private readonly ulong _botOwnerId;

        public RequireBotOwner(ulong userId)
        {
            _userId = userId;
            _botOwnerId = BotConfigDataHandler;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            if (_userId == _botOwnerId)
            {
                  return PreconditionResult.FromSuccess(); 
            }
            else return PreconditionResult.FromError($"You do not have the required permission to use this command.");
        }
    }
}