using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules.ManagingModule
{
    [Group("prefix")]
    public class PrefixCommands : ModuleBase<SocketCommandContext>
    {
        private GuildAccountService _guildAccountService;

        public PrefixCommands(GuildAccountService guildService = null)
        {
            _guildAccountService = guildService;
        }

        [Command("add")]
        public async Task AddGuildPrefix()
        {
            var guild = _guildAccountService.GetSettingsAccount(Context.Guild.Id);
            await Context.Channel.SendMessageAsync($"The previous prefixes were:");
        }
    }
}