using System;
using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Guilds;

namespace TheGoodBot.Core.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        [Command("Test")]
        public async Task TestAndStuff()
        {
            var guild = GuildAccounts.GetGuildAccount(Context.Guild.Id);
            guild.allMembersCombinedXP += 500;
            GuildAccounts.SaveAccount(guild, Context.Guild.Id);
            await Context.Channel.SendMessageAsync($"dab");
        }
    }
}