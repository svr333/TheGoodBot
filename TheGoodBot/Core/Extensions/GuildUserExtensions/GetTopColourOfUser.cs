using System.Linq;
using Discord;
using Discord.WebSocket;

namespace TheGoodBot.Core.Extensions
{
    public static class GetTopColourOfUser
    {
        public static Color GetUserTopColour(this SocketGuildUser user)
        {
            var hierarchyOrderedRoleList =
                user.Roles.OrderByDescending(x => x.Position).ToList();

            for (int i = 0; i < hierarchyOrderedRoleList.Count; i++)
            {
                if (hierarchyOrderedRoleList[i].Color == Color.Default) continue;
                else return hierarchyOrderedRoleList[i].Color;
            }
            return new Color(5198940);
        }
    }
}