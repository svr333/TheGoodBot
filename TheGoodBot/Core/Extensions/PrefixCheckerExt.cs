using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;

namespace TheGoodBot.Core.Extensions
{
    static class PrefixCheckerExt
    {
        public static bool HasPrefix(this SocketUserMessage message, DiscordSocketClient client, out int argPos,
            List<string> prefixes)
        {
            int prefixStart = 0;
            for (int i = 0; i < prefixes.Count(); i++)
            {
                argPos = prefixes[i].Length;
                if (message.HasStringPrefix(prefixes[i], ref prefixStart)) { return true; }
            }
            argPos = client.CurrentUser.Mention.Length;
            if (message.HasMentionPrefix(client.CurrentUser, ref prefixStart)) { return true; }
           else { return false; }
        }
    }
}