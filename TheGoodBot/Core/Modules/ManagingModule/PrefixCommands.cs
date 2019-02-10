using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
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
            var guild = _guildAccountService.GetOrCreateGuildAccountCategory(Context.Guild.Id, "Settings");
            await Context.Channel.SendMessageAsync($"The previous prefixes were:");
        }
    }
}