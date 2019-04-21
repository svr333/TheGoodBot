using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;

namespace TheGoodBot.Core.Modules.ModerationModule
{
    public class PurgeCommands : ModuleBase<SocketCommandContext>
    {
        private GuildAccountService _guildAccountService;

        public PurgeCommands(GuildAccountService guildAccountService)
        {
            _guildAccountService = guildAccountService;
        }

        [Cooldown]
        [Command("purge"), Alias()]
        [Summary("Basic purge command that purges x amount of messages.")]
        public async Task Purge(int num = 100, [Remainder]string reason = "No reason provided")
        {
            var messages = await Context.Channel.GetMessagesAsync(num).FlattenAsync();
            await DeleteMessages(Context, messages, num);
        }

        [Cooldown]
        [Command("purge user"), Alias()]
        [Summary("")]
        public async Task UserPurge(SocketUser user = null, int num = 100, [Remainder]string reason = "No reason provided")
        {
            user = user ?? Context.User;
            var messages = await Context.Channel.GetMessagesAsync(num).Where(x => x.FirstOrDefault().Author.Id == user.Id).FlattenAsync();
            await DeleteMessages(Context, messages, num);
        }

        private async Task DeleteMessages(SocketCommandContext context, IEnumerable<IMessage> messages, int num)
        {
            var settings = _guildAccountService.GetSettingsAccount(context.Guild.Id);
            if (settings.PurgePinnedMessages) messages = messages.Where(x => !x.IsPinned);
            await ((ITextChannel) context.Channel).DeleteMessagesAsync(messages);
        }
    }
}
