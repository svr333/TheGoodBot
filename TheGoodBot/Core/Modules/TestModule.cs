using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Core.Services.Languages;
using TheGoodOne.DataStorage;
using Newtonsoft.Json;

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

        [Cooldown]
        [Command("test"), Alias()]
        [Summary("Simple test command that does nothing but posting a message.")]
        public async Task TestAndStuff() =>
            await _customEmbedService.CreateAndPostEmbed(Context, "test");

        [Cooldown]
        [Command("guild"), Alias()]
        [Summary("Simple test command that does nothing but posting a message.")]
        public async Task Guild() =>
            await _customEmbedService.CreateAndPostEmbed(Context, "guild");

        [Command("ListRoles"), Alias()]
        [Summary("Lists all roles by id into a file.")]
        public async Task ListRoles()
        {
            Dictionary<string, string> roles = new Dictionary<string, string>();
            string filePath = $"Roles-{Context.Guild.Id}";

            for (int i = 0; i < Context.Guild.Roles.Count; i++)
            {
                roles.Add($"<@&{Context.Guild.Roles.ElementAt(i).Id}>", Context.Guild.Roles.ElementAt(i).Name);
            }
            var json = JsonConvert.SerializeObject(roles, Formatting.Indented);
            File.WriteAllText(filePath, json);
            await Context.Channel.SendFileAsync(filePath, "Here's a list of all the roles & their Id's.");
        }
    }
}
