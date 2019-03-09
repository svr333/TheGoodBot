using System;
using System.Collections.Generic;
using Discord;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Extensions
{
    public static class EmbedCreatorExt
    {
        public static Embed CreateEmbed(this CustomEmbed embed, out int createFieldFailAmount)
        {
            uint color = Convert.ToUInt32(embed.Colour, 16);
            var eb = new EmbedBuilder();
            eb.WithAuthor(embed.AuthorName, embed.AuthorIconUrl, embed.AuthorUrl);
            eb.WithColor(color);
            eb.WithFooter(embed.FooterText, embed.FooterUrl);
            eb.WithDescription(embed.Description);
            eb.WithImageUrl(embed.ImageUrl);
            eb.WithThumbnailUrl(embed.ThumbnailUrl);
            eb.WithTitle(embed.Title);
            eb.WithUrl(embed.EmbedUrl);

            int amountsFailed = 0;

            if (!(embed.FieldTitles == null))
            {
                var fields = embed.GetFields(out amountsFailed);

                for (int i = 0; i < fields.Count; i++)
                {
                    eb.AddField(fields[i].FieldTitle, fields[i].FieldValue, fields[i].InlineValue);
                }
            }

            if (eb.Length == 0)
            {
                createFieldFailAmount = 0;
                return null;
            }

            createFieldFailAmount = amountsFailed;
            eb.WithTimestamp(embed.TimeStamp);
            return eb.Build();
        }

        private static List<CustomField> GetFields(this CustomEmbed embed, out int amountsFailed)
        {
            var CustomFields = new List<CustomField>();
            int amountOfSucceededFields = 0;

            for (int i = 0; i < embed.FieldTitles.Length; i++)
            {
                if (embed.FieldTitles[i] == String.Empty || embed.FieldTitles[i] == null) { continue; }
                var field = new CustomField()
                {
                    FieldTitle = embed.FieldTitles[i],
                    FieldValue = embed.FieldValues[i],
                    InlineValue = embed.FieldInlineValues[i]
                };

                CustomFields.Add(field);
                amountOfSucceededFields++;
            }

            amountsFailed = embed.FieldTitles.Length - amountOfSucceededFields;
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