using System;
using Discord;

namespace TheGoodBot.Entities
{
    public class CustomEmbed
    {
        public string PlainText { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public Color Colour { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }
        public string EmbedUrl { get; set; }

        public string AuthorName { get; set; }
        public string AuthorIconUrl { get; set; }
        public string AuthorUrl { get; set; }

        public string FooterText { get; set; }
        public string FooterUrl { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string[] FieldTitles { get; set; }
        public object[] Values { get; set; }
        public bool[] InlineValues { get; set; }
    }
}