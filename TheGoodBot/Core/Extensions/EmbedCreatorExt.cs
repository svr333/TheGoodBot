using System;
using System.Collections.Generic;
using System.IO;
using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using TheGoodBot.Entities;
using TheGoodBot.Entities.GuildAccounts;

namespace TheGoodBot.Core.Extensions
{
    public static class EmbedCreatorExt
    {
        public static Embed CreateEmbed(this CustomEmbed embed, SocketGuildUser user)
        {
            // THIS IS DIRTY AF I KNOW BUT SSSSH
            var json = File.ReadAllText($"GuildUserAccounts/{user.Guild.Id}/{user.Id}.json");
            var guildUser = JsonConvert.DeserializeObject<GuildUserAccount>(json);
            json = File.ReadAllText($"GlobalUserAccounts/{user.Id}.json");
            var globalUser = JsonConvert.DeserializeObject<GlobalUserAccount>(json);
            json = File.ReadAllText($"GuildAccounts/{user.Guild.Id}/Settings.json");
            var guild = JsonConvert.DeserializeObject<Settings>(json);


            var eb = new EmbedBuilder();
            eb.WithAuthor(embed.AuthorName, embed.AuthorIconUrl, embed.AuthorUrl);
            eb.WithFooter(embed.FooterText, embed.FooterUrl);
            eb.WithDescription(embed.Description);
            eb.WithImageUrl(embed.ImageUrl);
            eb.WithThumbnailUrl(embed.ThumbnailUrl);
            eb.WithTitle(embed.Title);
            eb.WithUrl(embed.EmbedUrl);
            eb.WithTimestamp(embed.TimeStamp.GetValueOrDefault(DateTimeOffset.UtcNow));

            if (embed.FieldTitles != null)
            {
                var fields = embed.GetFields();

                for (int i = 0; i < fields.Count; i++)
                {
                    eb.AddField(fields[i].FieldTitle, fields[i].FieldValue, fields[i].InlineValue);
                }
            }

            if (eb.Length == 0) { return null; }

            if (guild.AllowMembersCustomEmbedColour && embed.Colour == 5198940)
            {
                if (guildUser.Colour == 5198940 && globalUser.Colour == 5198940) { eb.WithColor(user.GetUserTopColour()); }
                if (globalUser.Colour == 5198940 && guildUser.Colour != 5198940) { eb.WithColor(guildUser.Colour); }
                else if (globalUser.Colour != 5198940 && guildUser.Colour == 5198940) { eb.WithColor(globalUser.Colour); }
            }
            else { eb.WithColor(embed.Colour); }

            return eb.Build();
        }

        private static List<CustomField> GetFields(this CustomEmbed embed)
        {
            var CustomFields = new List<CustomField>();

            for (int i = 0; i < embed.FieldTitles.Length; i++)
            {
                if (embed.FieldTitles[i] == String.Empty || embed.FieldTitles[i] == null)
                {
                    embed.FieldTitles[i] = "\u200b";
                }

                var field = new CustomField()
                {
                    FieldTitle = embed.FieldTitles[i],
                    FieldValue = embed.FieldValues[i],
                    InlineValue = embed.FieldInlineValues[i]
                };

                CustomFields.Add(field);
            }

            return CustomFields;
        }
        private struct CustomField
        {
            public string FieldTitle;
            public object FieldValue;
            public bool InlineValue;
        }
    }
}
