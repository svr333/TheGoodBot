using System;
using System.IO;
using Discord;
using TheGoodBot.Entities;

namespace TheGoodBot.Languages
{
    public class ChangeCustomEmbedService
    {
        public void ValidateFile()
        {
            string filePath = "";
            string directory = "";

            if (File.Exists(filePath)) { return; }

            var embed = GenerateCustomEmbedStruct();

            Directory.CreateDirectory(directory);
            File.WriteAllText(filePath, "");
        }

        public void ChangeCustomEmbed()
        {

        }

        public CustomEmbedStruct GenerateCustomEmbedStruct() => new CustomEmbedStruct()
        {
            FieldTitles = null,
            FieldValues = null,
            FieldInlineValues = null,
            TimeStamp = DateTimeOffset.UtcNow,
            Title = String.Empty,
            Description = String.Empty,
            AuthorName = String.Empty,
            FooterText = String.Empty,
            AuthorIconUrl = String.Empty,
            Colour = Color.Blue,
            EmbedUrl = String.Empty,
            ThumbnailUrl = String.Empty,
            AuthorUrl = String.Empty,
            ImageUrl = String.Empty,
            FooterUrl = String.Empty,
            PlainText = String.Empty
        };
    }
}