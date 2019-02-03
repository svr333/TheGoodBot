﻿using System.Collections.Generic;
using System.Text;
using Discord;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Extensions
{
    public static class EmbedCreatorExt
    {
        private static List<CustomField> _embedFields;

        public static EmbedBuilder CreateEmbed(CustomEmbed embed, out int createFieldFailAmount)
        {
            var eb = new EmbedBuilder();
            eb.WithAuthor(embed.AuthorName, embed.AuthorIconUrl, embed.AuthorUrl);
            eb.WithColor(embed.Colour);
            eb.WithFooter(embed.FooterText, embed.FooterUrl);
            eb.WithDescription(embed.Description);
            eb.WithImageUrl(embed.ImageUrl);
            eb.WithThumbnailUrl(embed.ThumbnailUrl);
            eb.WithTitle(embed.Title);
            eb.WithUrl(embed.EmbedUrl);

            var fields = GetFields(embed, out int amountsFailed);

            for (int i = 0; i < fields.Count; i++)
            {
                foreach (var field in fields)
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

            if (embed.TimeStamp == null)
            {
                eb.WithCurrentTimestamp();
                return eb;
            }
            eb.WithTimestamp(embed.TimeStamp);
            return eb;
        }

        private static List<CustomField> GetFields(CustomEmbed embed, out int amountsFailed)
        {
            var CustomFields = new List<CustomField>();
            amountsFailed = 0;
            int amountOfSucceededFields = 0;
            CustomField field;

            for (int i = 0; i < embed.FieldTitles.Length; i++)
            {
                if (embed.FieldTitles[i] == string.Empty)
                {
                    amountsFailed++;
                    continue;
                }
                field.FieldTitle = embed.FieldTitles[i];
                field.FieldValue = embed.FieldValues[i];
                field.InlineValue = embed.FieldInlineValues[i];

                CustomFields[amountOfSucceededFields] = field;

                amountOfSucceededFields++;
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