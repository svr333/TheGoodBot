using Discord.Commands;
using System.Threading.Tasks;
using TheGoodBot.Core.Preconditions;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules.BotOwnerModule
{
    [RequireBotOwner]
    public class CreateFilesCommands : ModuleBase<SocketCommandContext>
    {
        private CreateGuildAccountFilesService _createGuildAccountFiles;

        public CreateFilesCommands(CreateGuildAccountFilesService createGuildAccountFiles)
        {
            _createGuildAccountFiles = createGuildAccountFiles;
        }

        [Cooldown, Invoke, RequireBotOwner]
        [Command("okay"), Alias()]
        [Summary("Simple test command that does nothing but posting a message.")]
        public async Task Okay()
        {
            _createGuildAccountFiles.CreateGuildAccount(Context.Guild.Id);
            await Context.Channel.SendMessageAsync("Files created");
        }
    }
}