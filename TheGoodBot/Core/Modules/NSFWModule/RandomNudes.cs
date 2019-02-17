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

        [Command("Butt"), Alias("butts, ass")]
        public async Task Butt()
        {
            var command = _commandService.Search(Context, "Butt").Commands.FirstOrDefault().Command;
            string[] array = new string[] { command.Name, command.Module.Name, command.Module.Group };

            var embed = _customEmbedService.GetAndConvertToDiscEmbed(Context.Guild.Id, Context.User.Id, array, out string text, out int amountsFailed);

            await Context.Channel.SendMessageAsync(text, false, embed);
        }
    }
}