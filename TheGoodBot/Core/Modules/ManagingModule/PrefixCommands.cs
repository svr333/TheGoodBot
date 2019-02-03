using System.Threading.Tasks;
using Discord.Commands;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules.ManagingModule
{
    [Group("prefix")]
    public class PrefixCommands : ModuleBase<SocketCommandContext>
    {
        private GuildAccountService _guildAccountService;

        public PrefixCommands(GuildAccountService guildAccountService)
        {
            _guildAccountService = guildAccountService;
        }


        [Command("add")]
        public async Task AddGuildPrefix()
        {
            var guild = _guildAccountService.GetOrCreateGuildAccount(Context.Guild.Id);
            await Context.Channel.SendMessageAsync($"The previous prefixes were:");
        }
    }
}