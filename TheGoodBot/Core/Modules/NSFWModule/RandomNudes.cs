using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Services.Languages;

namespace TheGoodBot.Core.Modules.NSFWModule
{
    [RequireNsfw]
    public class RandomNudes : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        private CustomEmbedService _customEmbedService;

        public RandomNudes(CommandService commandService, CustomEmbedService customEmbedService)
        {
            _commandService = commandService;
            _customEmbedService = customEmbedService;
        }

        [Command("butt"), Alias("butts, ass")]
        [Summary("Gets a random butt from the database and returns it.")]
        public async Task Butt() =>
            await _customEmbedService.CreateAndPostEmbed(Context, "butt");
    }
}