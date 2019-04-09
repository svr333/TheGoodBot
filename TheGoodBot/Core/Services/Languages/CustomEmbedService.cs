using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Entities;
using TheGoodBot.Languages;

namespace TheGoodBot.Core.Services.Languages
{
    public class CustomEmbedService
    {
        private LanguageService _languageService;
        private CommandService _commandService;
        private JsonFormatter _formatService;

        public CustomEmbedService(LanguageService languageService, CommandService commandService, JsonFormatter formatService)
        {
            _languageService = languageService;
            _commandService = commandService;
            _formatService = formatService;
        }

        private LanguageObject GetLanguageObject(ulong guildID, ulong userID, string[] commandInfo)
        {
            string commandName = commandInfo[0];
            string moduleName = commandInfo[1];
            string groupName = commandInfo[2];
            string name = String.Empty;

            if (groupName == String.Empty || groupName == null) { name = commandName; }
            else { name = groupName + "-" + commandName; }

            var language = _languageService.GetLanguage(guildID, userID);
            var filePath = $"Languages/{language}/{moduleName}/{name}.json";

            var text = File.ReadAllText(filePath);
            var languageObject = _formatService.GetFormattedEmbeds(guildID, userID, commandName, text);
            return languageObject;
        }

        private List<Embed> GetAndConvertToDiscEmbeds(ulong guildID, SocketGuildUser user, string[] commandInfo, out string ChnText, out string DmText)
        {
            List<Embed> embeds = new List<Embed>();

            var languageObject = GetLanguageObject(guildID, user.Id, commandInfo);
            var ChnEmbed = languageObject.ChnEmbed.CreateEmbed(user);
            var DmEmbed = languageObject.DmEmbed.CreateEmbed(user);

            embeds.Add(ChnEmbed);
            embeds.Add(DmEmbed);

            ChnText = languageObject.ChnEmbed.PlainText;
            DmText = languageObject.DmEmbed.PlainText;
            return embeds;
        }

        public async Task CreateAndPostEmbeds(SocketCommandContext context, string name)
        {
            var commandContext = _commandService.Search(context, name);
            string[] commandInfo;

            if (commandContext.Commands == null) { commandInfo = new string[] { name, "!UnchangeableEmbeds", ""}; }
            else
            {
                var command = commandContext.Commands.FirstOrDefault().Command;
                commandInfo = new string[] {command.Name, command.Module.Name, command.Module.Group};
            }

            var embeds = GetAndConvertToDiscEmbeds(context.Guild.Id, (SocketGuildUser) context.User, commandInfo, out string ChnText, out string DmText);

            if (embeds[0] != null || ChnText != null && ChnText != "")
            {
                await context.Channel.SendMessageAsync(ChnText, false, embeds[0]);
            }
            if (embeds[1] != null || DmText != null && DmText != "")
            {
                await context.User.SendMessageAsync(ChnText, false, embeds[1]);
            }
        }
    }
}