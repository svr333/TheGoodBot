using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("Test"), RequireBotOwner(540830993196515328)]
        public async Task TestAndStuff()
        {
            var guild = GuildAccounts.GetGuildAccount(Context.Guild.Id);
            guild.allMembersCombinedXP += 500;
            GuildAccounts.SaveAccount(guild, Context.Guild.Id);
            await Context.Channel.SendMessageAsync($"dab");
        }
    }
}