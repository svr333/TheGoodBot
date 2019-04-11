using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Commands;
using TheGoodBot.Core.Services.Languages;

namespace TheGoodBot.Core.Modules.ManagingModule
{
    public class PrefixCommands : ModuleBase<SocketCommandContext>
    {
        private PrefixService _prefixService;
        private CustomEmbedService _embedService;

        public PrefixCommands(PrefixService prefixService, CustomEmbedService embedService)
        {
            _prefixService = prefixService;
            _embedService = embedService;
        }

        [Cooldown]
        [Command("prefix"), Alias("prefixes")]
        [Summary("Shows the guild's prefixes.")]
        public async Task ListGuildPrefixes()
            => await _embedService.CreateAndPostEmbeds(Context, "prefix");

        [Cooldown]
        [Command("addprefix"), Alias("prefix add", "aprefix")]
        [Summary("Adds a certain prefix to the guild's prefixes.")]
        public async Task AddGuildPrefix([Remainder]string prefix = "")
            => _prefixService.AddPrefixAsync(Context, prefix);

        [Cooldown]
        [Command("removeprefix"), Alias("prefix remove", "rprefix", "deleteprefix", "delprefix")]
        [Summary("Removes a certain prefix from the guild's prefixes.")]
        public async Task RemoveGuildPrefix([Remainder]string prefix = "")
            => _prefixService.RemovePrefixAsync(Context, prefix);
    }
}
