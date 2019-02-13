using System.Linq;
using Discord;

namespace TheGoodBot.Core.Extensions
{
    public static class UserHasRoleExt
    {
        public static bool UserHasRole(this IGuildUser guildUser, ulong roleID)
        {
            for (int i = 0; i < guildUser.RoleIds.Count; i++)
            {
                if (guildUser.RoleIds.ToList()[i] == roleID) { return true; }
                else { return false; }
            }

            return false;
        }
    }
}