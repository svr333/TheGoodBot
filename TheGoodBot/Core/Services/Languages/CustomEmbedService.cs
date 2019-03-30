using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private JsonFormatter _formatService;

        public CustomEmbedService(LanguageService languageService, CommandService commandService, JsonFormatter formatService)
        {
            _languageService = languageService;
            _commandService = commandService;
            _formatService = formatService;
        }

        private CustomEmbed GetCustomEmbed(ulong guildID, ulong userID, string[] commandInfo)
        {
            string commandName = commandInfo[0];
            string moduleName = commandInfo[1];
            string groupName = commandInfo[2];
            string name = String.Empty;

            if (groupName == String.Empty || groupName == null) { name = commandName; }
            else { name = groupName + "-" + commandName; }

            var language = _languageService.GetLanguage(guildID, userID);
            if (language == null || language == String.Empty) { language = "English"; }
            var filePath = $"Languages/{language}/{moduleName}/{name}.json";

            var text = File.ReadAllText(filePath);
            var customEmbed = _formatService.GetFormattedEmbed(guildID, userID, commandName, text);
            return customEmbed;
        }

        private Embed GetAndConvertToDiscEmbed(ulong guildID, ulong userID, string[] commandInfo, out string text, out int amountsFailed)
        {
            var customEmbed = GetCustomEmbed(guildID, userID, commandInfo);
            var embed = customEmbed.CreateEmbed(out int createFieldFailAmount);
            text = customEmbed.PlainText;
            amountsFailed = createFieldFailAmount;
            return embed;
        }

        private CustomEmbed GetAndChangeEmbed(ulong guildID, ulong userID, string[] commandInfo)
        {
            var customEmbed = GetCustomEmbed(guildID, userID, commandInfo);
            return customEmbed;
        }

        public async Task CreateAndPostEmbed(SocketCommandContext context, string name)
        {
            var commandContext = _commandService.Search(context, name);
            string[] commandInfo;

            if (commandContext.Commands == null) { commandInfo = new string[] { name, "!UnchangeableEmbeds", ""}; }
            else
            {
                var command = commandContext.Commands.FirstOrDefault().Command;
                commandInfo = new string[] {command.Name, command.Module.Name, command.Module.Group};
            }

            var embed = GetAndConvertToDiscEmbed(context.Guild.Id, context.User.Id, commandInfo, out string text, out int amountsFailed);

            if (embed != null || text != null && text != "")
            {
                await context.Channel.SendMessageAsync(text, false, embed);
                if (!(amountsFailed == 0))
                {
                    await CreateAndPostEmbed(context, "FieldFailure");
                }
            }
        }
    }
}