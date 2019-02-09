using System.Linq;
using Discord.Commands;
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

        /*public CustomEmbedStruct GetCustomEmbed(ulong guildID, ulong userID, string moduleName, string commandName)
        {
            var language = _languageService.GetLanguage(guildID, userID);
            var filePath = "Languages/" + language + "/" + moduleName + "" + commandName;
        }*/
    }
}