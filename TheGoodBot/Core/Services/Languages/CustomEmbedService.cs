using System;
using System.IO;
using Discord;
using Discord.Commands;
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

        public CustomEmbedService(LanguageService languageService = null, CommandService commandService = null)
        {
            _languageService = languageService;
            _commandService = commandService;
        }

        private CustomEmbedStruct GetCustomEmbed(ulong guildID, ulong userID, string[] commandInfo)
        {
            string commandName = commandInfo[0];
            string moduleName = commandInfo[1];
            string groupName = commandInfo[2];
            string name = String.Empty;

            if (groupName == null) { name = commandName; }
            else { name = groupName + "-" + commandName; }

            var language = _languageService.GetLanguage(guildID, userID);
            if (language == null || language == String.Empty) { language = "English"; }
            var filePath = "Languages/" + language + "/" + moduleName + "/" + name + ".json";

            var json = File.ReadAllText(filePath);
            var customEmbed = JsonConvert.DeserializeObject<CustomEmbedStruct>(json);
            return customEmbed;
        }

        public Embed GetAndCreateEmbed(ulong guildID, ulong userID, string[] commandInfo, out string text, out int amountsFailed)
        {
            var customEmbed = GetCustomEmbed(guildID, userID, commandInfo);
            var embed = customEmbed.CreateEmbed(out int createFieldFailAmount);
            text = customEmbed.PlainText;
            amountsFailed = createFieldFailAmount;
            return embed;
        }
    }
}