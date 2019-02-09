using System;
using System.IO;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Languages
{
    public class RetrieveCustomEmbedService
    {
        private LanguageService _languageService;
        private CommandService _commandService;

        public RetrieveCustomEmbedService(LanguageService languageService = null, CommandService commandService = null)
        {
            _languageService = languageService;
            _commandService = commandService;
        }

        public CustomEmbedStruct GetCustomEmbed(ulong guildID, ulong userID, string[] commandInfo)
        {
            string commandName = commandInfo[0];
            string moduleName = commandInfo[1];
            string groupName = commandInfo[2];
            string name = String.Empty;

            if (groupName == String.Empty) { name = commandName; }
            else { name = groupName + "-" + commandName; }

            var language = _languageService.GetLanguage(guildID, userID);
            var filePath = "Languages/" + language + "/" + moduleName + "" + name;

            var json = File.ReadAllText(filePath);
            var customEmbed = (CustomEmbedStruct)JsonConvert.DeserializeObject(json);
            return customEmbed;
        }
    }
}