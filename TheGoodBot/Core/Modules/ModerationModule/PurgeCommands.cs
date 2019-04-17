using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using TheGoodBot.Core.Preconditions;

namespace TheGoodBot.Core.Modules.ModerationModule
{
    [Group("purge")]
    public class PurgeCommands : ModuleBase<SocketCommandContext>
    {
        public PurgeCommands()
        {

        }

        [Cooldown]
        [Command(""), Alias()]
        [Summary("Basic purge command that purges x amount of messages.")]
        public async Task Purge(int num, [Remainder]string reason = "No reason provided")
        {
            var messages = await Context.Channel.GetMessagesAsync(num).FlattenAsync();
            await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
        }
    }
}
