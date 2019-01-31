using System.Collections.Generic;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public static class GuildAccounts
    {
        //(auto-)creation of GuildAccounts
        private static List<GuildAccountStruct> _guildAccounts;

        private static string saveFolder = "GuildAccounts";

        static GuildAccounts()
        {
            if (GuildAccountsDataHandler.SaveExists(saveFolder))
            {
                
            }

            
        }
    }
}