﻿using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Extensions
{
    public static class EmbedCreatorExt
    {
        public static Embed CreateEmbed(CustomEmbedStruct embed, out int createFieldFailAmount)
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

            int amountsFailed = 0;

            if (!(embed.FieldTitles == null))
            {
                var fields = GetFields(embed, out int amountsFieldFailed);

                for (int i = 0; i < fields.Count; i++)
                {
                        eb.AddField(fields[i].FieldTitle, fields[i].FieldValue, fields[i].InlineValue);
                }

                amountsFieldFailed = amountsFailed;
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

        private static List<CustomField> GetFields(CustomEmbedStruct embed, out int amountsFailed)
        {
            var CustomFields = new List<CustomField>();
            amountsFailed = 0;
            int amountOfSucceededFields = 0;

            for (int i = 0; i < embed.FieldTitles.Length; i++)
            {
                if (embed.FieldTitles[i] == string.Empty)
                {
                    amountsFailed++;
                    continue;
                }
                var field = new CustomField()
                {
                    FieldTitle = embed.FieldTitles[i],
                    FieldValue = embed.FieldValues[i],
                    InlineValue = embed.FieldInlineValues[i]
                };

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