using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TheGoodBot.Core.Extensions;
using TheGoodBot.Entities;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Core.Services.Languages
{
    public class JsonFormatter
    {
        private GuildAccountService _guildAccount;
        private ulong _guildID;
        private ulong _userID;
        private string _commandName;

        public JsonFormatter(GuildAccountService guildAccount)
        {
            _guildAccount = guildAccount;
        }

        public CustomEmbed GetFormattedEmbed(ulong guildID, ulong userID, string commandName, string unformattedText)
        {
            _guildID = guildID;
            _userID = userID;
            _commandName = commandName;

            var unformattedEmbed = JsonConvert.DeserializeObject<CustomEmbed>(unformattedText);


            var formattedEmbed = new CustomEmbed()
            {
                PlainText = StringFormatter(unformattedEmbed.PlainText),
                AuthorIconUrl = StringFormatter(unformattedEmbed.AuthorIconUrl),
                AuthorName = StringFormatter(unformattedEmbed.AuthorName),
                AuthorUrl = StringFormatter(unformattedEmbed.AuthorUrl),
                Description = StringFormatter(unformattedEmbed.Description),
                EmbedUrl = StringFormatter(unformattedEmbed.EmbedUrl),
                FooterText = StringFormatter(unformattedEmbed.FooterText),
                FooterUrl = StringFormatter(unformattedEmbed.FooterText),
                ImageUrl = StringFormatter(unformattedEmbed.ImageUrl),
                ThumbnailUrl = StringFormatter(unformattedEmbed.ThumbnailUrl),
                Title = StringFormatter(unformattedEmbed.Title),
                FieldTitles = StringFormatter(unformattedEmbed.FieldTitles),
                FieldValues = StringFormatter(unformattedEmbed.FieldTitles),
                FieldInlineValues = unformattedEmbed.FieldInlineValues,
                Colour =  unformattedEmbed.Colour,
                TimeStamp = unformattedEmbed.TimeStamp
            };

            return formattedEmbed;
        }

        private string StringFormatter(string formattedText)
        {
            var guildAccount = _guildAccount.GetSettingsAccount(_guildID);
            var statsAccount = _guildAccount.GetStatsAccount(_guildID);
            var cooldown = _guildAccount.GetCooldown(_commandName, _guildID);
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
                    case "Guild.TotalXP": parameters[i] = statsAccount.AllMembersCombinedXP.ToString();
                        break;
                    case "Guild.Messages": parameters[i] = statsAccount.AllMembersMessagesSent.ToString();
                        break;
                    case "Guild.Cooldown": parameters[i] = cooldown.ToString();
                        break;
                    default: parameters[i] = "[Could not find this setting. Please check your language files.]";
                        break;
                }
            }

            return String.Format(sb.ToString(), parameters);
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

            var formattedTextArray = new string[] { };
            formattedTextArray = list.ToArray();
            return formattedTextArray;
        }
    }
}