using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Core.Services.Languages;
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
        private CommandService _commandService;
        private LanguageService _languageService;
        private CustomEmbedService _customEmbedService;
        private CreateGuildAccountFilesService _createGuildAccountFiles;

        public TestModule(GuildAccountService guildAccountService, GuildUserAccountService guildUserService = null,
            GlobalUserAccountService globalUserService = null, LanguageService languageService = null,
            CommandService Command = null, CustomEmbedService customEmbedService = null, CreateGuildAccountFilesService createGuildAccountFiles = null)
        {
            _guildAccountService = guildAccountService;
            _guildUserAccountService = guildUserService;
            _globalUserAccountService = globalUserService;
            _commandService = Command;
            _languageService = languageService;
            _customEmbedService = customEmbedService;
            _createGuildAccountFiles = createGuildAccountFiles;
        }

        [Command("Test")]
        public async Task TestAndStuff()
        {
            var command = _commandService.Search(Context, "Test").Commands.FirstOrDefault().Command;
            string[] array = new string[] { command.Name, command.Module.Name, command.Module.Group };

            var embed = _customEmbedService.GetAndCreateEmbed(Context.Guild.Id, Context.User.Id, array, out string text, out int amountsFailed);   

            if (!(embed == null))
            {
                await Context.Channel.SendMessageAsync(text, false, embed);
                if (!(amountsFailed == 0))
                {
                    await Context.Channel.SendMessageAsync($"There were titles missing to create other fields. Failures: `{amountsFailed}`");
                }  
            }
        }

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
