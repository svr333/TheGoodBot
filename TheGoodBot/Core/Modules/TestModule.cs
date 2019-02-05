using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Guilds;
using TheGoodBot.Languages;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Modules
{
    public class TestModule : ModuleBase<SocketCommandContext>
    {
        private GuildAccountService _guildAccountService;
        private GuildUserAccountService _guildUserAccountService;
        private GlobalUserAccountService _globalUserAccountService;
        private LanguageSelector _languageSelector;
        private ChangeCustomEmbedService _changeCustomEmbedService;

        public TestModule(GuildAccountService guildService = null, GuildUserAccountService guildUserService = null, GlobalUserAccountService globalUserService = null, LanguageSelector languageSelector = null, ChangeCustomEmbedService changeCustomEmbedService = null)
        {
            _guildAccountService = guildService;
            _guildUserAccountService = guildUserService;
            _globalUserAccountService = globalUserService;
            _languageSelector = languageSelector;
            _changeCustomEmbedService = changeCustomEmbedService;
        }

        [Command("Test"), RequireBotOwner()]
        public async Task TestAndStuff()
        {
            uint lol = 0x858585;
            var guild = _guildAccountService.GetOrCreateGuildAccount(Context.Guild.Id);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(Context.Guild.Id, Context.User.Id);
            guild.AllMembersCombinedXP += 500;
            _guildAccountService.SaveGuildAccount(guild, Context.Guild.Id);
            Console.WriteLine(Color.Red.ToString());
            await Context.Channel.SendMessageAsync(guild.GuildID.ToString() + "||||||||||||||" + guildUser.UserId.ToString());
        }
    }
}