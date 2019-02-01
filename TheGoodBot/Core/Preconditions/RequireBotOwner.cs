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
        private BotConfigService _configService;

        public RequireBotOwner(ulong userId)
        {
            _userId = userId;
            _configService = new BotConfigService();
            _botOwnerId = _configService.GetConfig().botOwnerID;
            Console.WriteLine(_botOwnerId);
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