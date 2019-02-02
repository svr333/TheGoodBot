using System.Threading.Tasks;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private GuildAccountService _guildAccountService;
        private GuildUserAccountService _guildUserAccountService;

        public TestModule(GuildAccountService guildService = null, GuildUserAccountService guildUserService = null)
        {
            _guildAccountService = guildService;
            _guildUserAccountService = guildUserService;
        }

        [Command("Test"), RequireBotOwner()]
        public async Task TestAndStuff()
        {
            var guild = _guildAccountService.GetOrCreateGuildAccount(Context.Guild.Id);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(Context.Guild.Id, Context.User.Id);
            guild.allMembersCombinedXP += 500;
            _guildAccountService.SaveGuildAccount(guild, Context.Guild.Id);
            await Context.Channel.SendMessageAsync(guild.guildID.ToString() + "||||||||||||||" + guildUser.UserId.ToString());
        }

        [Command("NewCommand")]
        public async Task Command()
        {
            await Context.Channel.SendMessageAsync($"I can do it");
        }
    }
}