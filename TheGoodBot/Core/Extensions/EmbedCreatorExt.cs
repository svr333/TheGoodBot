using Discord;
using TheGoodBot.Entities;

namespace TheGoodBot.Core.Extensions
{
    public static class EmbedCreatorExt
    {
        public static EmbedBuilder CreateEmbed(CustomEmbed embed)
        {
            var eb = new EmbedBuilder();
            eb.WithAuthor(embed.AuthorName, embed.AuthorIconUrl, embed.AuthorUrl);





            if (eb.Length == 0)
            {
                return null;
            }
            return eb;
        }
    }
}