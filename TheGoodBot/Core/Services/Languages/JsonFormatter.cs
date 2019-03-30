using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
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
                //Title = StringFormatter(unformattedEmbed.Title),
                //FieldTitles = StringFormatter(unformattedEmbed.FieldTitles),
                //FieldValues = StringFormatter(unformattedEmbed.FieldTitles),
                //FieldInlineValues = unformattedEmbed.FieldInlineValues,
                //Colour =  unformattedEmbed.Colour,
                //TimeStamp = unformattedEmbed.TimeStamp
            };

            return formattedEmbed;
        }

        private string StringFormatter(string formattedText)
        {
            var Guild = _guildAccount.GetSettingsAccount(_guildID);
            var Stats = _guildAccount.GetStatsAccount(_guildID);
            var bla = _guildAccount.GetCooldown(_commandName, _guildID);
            var sb = new StringBuilder();
            var list = new List<string>();

            string[] separations = formattedText.Split(new[] { '{', '}' });
            for (int i = 0; i < separations.Length; i++)
            {
                if (i % 2 == 0) { sb.Append(separations[i]); }
                else
                {
                    sb.Append($"{{{0}}}");
                    list.Add(separations[i]);
                }
            }

            string[] parameters = new string[] { };
            parameters = list.ToArray();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ToString() == "Guild.GuildID")
                {
                    parameters[i] = Guild.GuildID.ToString();
                }
            }

            return String.Format(sb.ToString(), parameters);
        }

        private string[] StringFormatter(string[] unformattedTextArray)
        {
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