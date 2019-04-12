using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Entities;
using TheGoodBot.Languages;

namespace TheGoodBot.Core.Services.Languages
{
    public class CustomEmbedService
    {
        private readonly LanguageService _languageService;
        private readonly CommandService _commandService;
        private readonly JsonFormatter _formatService;

        public CustomEmbedService(LanguageService languageService, CommandService commandService, JsonFormatter formatService)
        {
            _languageService = languageService;
            _commandService = commandService;
            _formatService = formatService;
        }

        private LanguageObject GetLanguageObject(SocketCommandContext context, string[] commandInfo)
        {
            var commandName = commandInfo[0];
            var moduleName = commandInfo[1];
            var groupName = commandInfo[2];
            var name = string.Empty;

            if (string.IsNullOrEmpty(groupName)) { name = commandName; }
            else { name = groupName + "-" + commandName; }

            var language = _languageService.GetLanguage(context.Guild.Id, context.User.Id);
            var filePath = $"Languages/{language}/{moduleName}/{name}.json";

            var text = File.ReadAllText(filePath);
            var languageObject = _formatService.GetFormattedEmbeds(context, commandName, text);
            return languageObject;
        }

        private List<Embed> GetAndConvertToDiscEmbeds(SocketCommandContext context, string[] commandInfo, out string ChnText, out string DmText)
        {
            List<Embed> embeds = new List<Embed>();

            var languageObject = GetLanguageObject(context, commandInfo);
            var ChnEmbed = languageObject.ChnEmbed.CreateEmbed((SocketGuildUser) context.User);
            var DmEmbed = languageObject.DmEmbed.CreateEmbed((SocketGuildUser) context.User);

            embeds.Add(ChnEmbed); embeds.Add(DmEmbed);

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

            var embeds = GetAndConvertToDiscEmbeds(context, commandInfo, out string ChnText, out string DmText);

            if (embeds[0] != null || string.IsNullOrEmpty(ChnText))
            {
                await context.Channel.SendMessageAsync(ChnText, false, embeds[0]);
            }
            if (embeds[1] != null || string.IsNullOrEmpty(DmText))
            {
                await context.User.SendMessageAsync(ChnText, false, embeds[1]);
            }
        }
    }
}