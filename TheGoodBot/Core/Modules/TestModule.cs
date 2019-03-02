using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private CustomEmbedService _customEmbedService;
        private CreateGuildAccountFilesService _createGuildAccountFiles;
        private GuildAccountService _guildAccountService;

        public TestModule(CustomEmbedService customEmbedService = null, CreateGuildAccountFilesService createGuildAccountFiles = null,
            GuildAccountService guildAccountService = null)
        {
            _customEmbedService = customEmbedService;
            _createGuildAccountFiles = createGuildAccountFiles;
            _guildAccountService = guildAccountService;
        }

        [Command("Test"), Cooldown()]
        public async Task TestAndStuff() => await _customEmbedService.CreateAndPostEmbed(Context, "Test");

        [Command("Guild")]
        public async Task Guild()
        {
            var test = _guildAccountService.GetSettingsAccount(Context.Guild.Id);
            await Context.Channel.SendMessageAsync(test.GuildID.ToString());
        }

        [Command("Purge")]
        public async Task Purge(int num)
        {
            var messages = await Context.Channel.GetMessagesAsync(num).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }

        [Command("okay")]
        public async Task Okay()
        {
            _createGuildAccountFiles.CreateGuildAccount(Context.Guild.Id);
            await Context.Channel.SendMessageAsync("Files created");
        }
    }
}
