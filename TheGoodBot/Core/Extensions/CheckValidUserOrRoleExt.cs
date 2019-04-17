using Discord;
using System.Collections.Generic;
using Discord.Commands;

namespace TheGoodBot.Core.Extensions
{
    public static class CheckValidUserOrRoleExt
    {
        public static bool ValidatePermissions(this List<ulong> UsersAndRoles, ICommandContext context)
        {
            var guildUser = (IGuildUser) context.User;

            if (UsersAndRoles == null) { return false; }

            for (int i = 0; i< UsersAndRoles.Count; i++)
            {
                if (context.User.Id == UsersAndRoles[i] || guildUser.UserHasRole(UsersAndRoles[i])) { return true; }
            }
            return false;
        }
    }
}
