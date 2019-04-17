using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Services.Languages
{
    public class JsonFormatter
    {
        private readonly GuildAccountService _guildAccount;
        private SocketCommandContext _context;
        private GlobalUserCooldowns _globalUserCooldowns;
        private DiscordSocketClient _client;
        private CommandService _command;

        private string _commandName;

        public JsonFormatter(GuildAccountService guildAccount, GlobalUserCooldowns globalUserCooldowns, DiscordSocketClient client,
            CommandService command)
        {
            _guildAccount = guildAccount;
            _globalUserCooldowns = globalUserCooldowns;
            _client = client;
            _command = command;
        }

        public LanguageObject GetFormattedEmbeds(SocketCommandContext context, string commandName, string unformattedText)
        {
            _context = context;
            _commandName = commandName;

            var unformattedEmbed = JsonConvert.DeserializeObject<LanguageObject>(unformattedText);


            var formattedEmbeds = new LanguageObject()
            {
                ChnEmbed = new CustomEmbed()
                {
                    PlainText = StringFormatter(unformattedEmbed.ChnEmbed.PlainText),
                    AuthorIconUrl = StringFormatter(unformattedEmbed.ChnEmbed.AuthorIconUrl),
                    AuthorName = StringFormatter(unformattedEmbed.ChnEmbed.AuthorName),
                    AuthorUrl = StringFormatter(unformattedEmbed.ChnEmbed.AuthorUrl),
                    Description = StringFormatter(unformattedEmbed.ChnEmbed.Description),
                    EmbedUrl = StringFormatter(unformattedEmbed.ChnEmbed.EmbedUrl),
                    FooterText = StringFormatter(unformattedEmbed.ChnEmbed.FooterText),
                    FooterUrl = StringFormatter(unformattedEmbed.ChnEmbed.FooterText),
                    ImageUrl = StringFormatter(unformattedEmbed.ChnEmbed.ImageUrl),
                    ThumbnailUrl = StringFormatter(unformattedEmbed.ChnEmbed.ThumbnailUrl),
                    Title = StringFormatter(unformattedEmbed.ChnEmbed.Title),
                    FieldTitles = StringFormatter(unformattedEmbed.ChnEmbed.FieldTitles),
                    FieldValues = StringFormatter(unformattedEmbed.ChnEmbed.FieldTitles),
                    FieldInlineValues = unformattedEmbed.ChnEmbed.FieldInlineValues,
                    Colour = unformattedEmbed.ChnEmbed.Colour,
                    TimeStamp = unformattedEmbed.ChnEmbed.TimeStamp
                },
                DmEmbed = new CustomEmbed()
                {
                    PlainText = StringFormatter(unformattedEmbed.DmEmbed.PlainText),
                    AuthorIconUrl = StringFormatter(unformattedEmbed.DmEmbed.AuthorIconUrl),
                    AuthorName = StringFormatter(unformattedEmbed.DmEmbed.AuthorName),
                    AuthorUrl = StringFormatter(unformattedEmbed.DmEmbed.AuthorUrl),
                    Description = StringFormatter(unformattedEmbed.DmEmbed.Description),
                    EmbedUrl = StringFormatter(unformattedEmbed.DmEmbed.EmbedUrl),
                    FooterText = StringFormatter(unformattedEmbed.DmEmbed.FooterText),
                    FooterUrl = StringFormatter(unformattedEmbed.DmEmbed.FooterText),
                    ImageUrl = StringFormatter(unformattedEmbed.DmEmbed.ImageUrl),
                    ThumbnailUrl = StringFormatter(unformattedEmbed.DmEmbed.ThumbnailUrl),
                    Title = StringFormatter(unformattedEmbed.DmEmbed.Title),
                    FieldTitles = StringFormatter(unformattedEmbed.DmEmbed.FieldTitles),
                    FieldValues = StringFormatter(unformattedEmbed.DmEmbed.FieldTitles),
                    FieldInlineValues = unformattedEmbed.DmEmbed.FieldInlineValues,
                    Colour = unformattedEmbed.DmEmbed.Colour,
                    TimeStamp = unformattedEmbed.ChnEmbed.TimeStamp
                }
                
            };

            return formattedEmbeds;
        }

        private string StringFormatter(string formattedText)
        {
            var command = _command.Search(_context, _commandName);
            var commandInfo = command.Commands.FirstOrDefault().Command;

            var guildAccount = _guildAccount.GetSettingsAccount(_context.Guild.Id);
            var statsAccount = _guildAccount.GetStatsAccount(_context.Guild.Id);

            var cooldown = _guildAccount.GetMaxCooldown(_commandName, _context.Guild.Id);
            _globalUserCooldowns.GetUserCooldown($"{commandInfo.GetHashCode()}-{_context.User.Id}", out DateTime endsAt);
            var difference = endsAt.Subtract(DateTime.UtcNow);


            var commandTime = (DateTime.Now - _context.Message.Timestamp).TotalMilliseconds;
            var sb = new StringBuilder();
            var list = new List<string>();

            string[] separations = formattedText.Split(new[] { '{', '}' });
            for (int i = 0; i < separations.Length; i++)
            {
                if (i % 2 == 0) { sb.Append(separations[i]); }
                else
                {
                    sb.Append($"{{{i/2}}}");
                    list.Add(separations[i]);
                }
            }

            string[] parameters = new string[] { };
            parameters = list.ToArray();

            for (int i = 0; i < parameters.Length; i++)
            {
                switch (parameters[i])
                {
                    case "Guild.GuildID": parameters[i] = guildAccount.GuildID.ToString();
                        break;
                    case "Guild.Prefixes": parameters[i] = guildAccount.PrefixList.ReturnListAsString();
                        break;
                    case "Guild.Language": parameters[i] = guildAccount.Language;
                        break;
                    case "Guild.CommandsExecuted": parameters[i] = statsAccount.AllMembersCommandsExecuted.ToString();
                        break;
                    case "Guild.TotalXP": parameters[i] = statsAccount.AllMembersCombinedXp.ToString();
                        break;
                    case "Guild.Messages": parameters[i] = statsAccount.AllMembersMessagesSent.ToString();
                        break;
                    case "Guild.Cooldown": parameters[i] = cooldown.ToString();
                        break;
                    case "Command.Time": parameters[i] = commandTime.ToString();
                        break;
                    case "Cooldown.EndsIn": parameters[i] = difference.ToString(@"mm\m\:ss\s");
                        break;
                    case "Cooldown.EndsInLong":
                        parameters[i] = difference.ToString(@"mm\ \m\i\n\u\t\e\s\ \a\n\d\ ss\ \s\e\c\o\n\d\s");
                        break;
                    default: parameters[i] = "[Could not find this setting. Please check your language files.]";
                        break;
                }
            }

            return string.Format(sb.ToString(), parameters);
        }

        private string[] StringFormatter(string[] unformattedTextArray)
        {
            if (unformattedTextArray is null) { return null; }

            var list = new List<string>();

            for (int i = 0; i < unformattedTextArray.Length; i++)
            {
                var formattedText = StringFormatter(unformattedTextArray[i]);
                list.Add(formattedText);
            }

            return list.ToArray();
        }
    }
}
