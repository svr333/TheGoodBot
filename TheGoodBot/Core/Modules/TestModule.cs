using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Entities;
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
        private CommandService _commandService;

        public TestModule(GuildAccountService guildService = null, GuildUserAccountService guildUserService = null,
            GlobalUserAccountService globalUserService = null, LanguageSelector languageSelector = null,
            ChangeCustomEmbedService changeCustomEmbedService = null, CommandService Command = null)
        {
            _guildAccountService = guildService;
            _guildUserAccountService = guildUserService;
            _globalUserAccountService = globalUserService;
            _languageSelector = languageSelector;
            _changeCustomEmbedService = changeCustomEmbedService;
            _commandService = Command;
        }

        [Command("Test"), RequireBotOwner()]
        public async Task TestAndStuff()
        {
            var customembed = new CustomEmbedStruct();
            var embed = EmbedCreatorExt.CreateEmbed(customembed, out int amountsFailed);        

            if (!(embed == null))
            {
                await Context.Channel.SendMessageAsync("", false, embed);
                await Context.Channel.SendMessageAsync($"Amounts failed = {amountsFailed}");
                Console.WriteLine("embed was null");
            }          
        }


        [Command("Guild")]
        public async Task Guild()
        {
            var test = _commandService.Commands.ToList();
            for (int i = 0; i < test.Count; i++)
            {
                Console.WriteLine(test[i].Name);
                Console.WriteLine(test.Count);
            }

            var guild = _guildAccountService.GetOrCreateGuildAccount(Context.Guild.Id);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(Context.Guild.Id, Context.User.Id);
            guild.AllMembersCombinedXP += 500;
            _guildAccountService.SaveGuildAccount(guild, Context.Guild.Id);

            await Context.Channel.SendMessageAsync(guild.GuildID.ToString() + "||||||||||||||" + guildUser.UserId.ToString());

            
        }
    }
}