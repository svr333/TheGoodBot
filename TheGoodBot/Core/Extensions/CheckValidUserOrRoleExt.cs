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

            for (int i = 0; i< UsersAndRoles.Count; i++)
            {
                if (context.User.Id == UsersAndRoles[i] || guildUser.UserHasRole(UsersAndRoles[i])) { return true; }
            }
            return false;
        }
    }
}