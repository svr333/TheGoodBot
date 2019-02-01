using System.Collections.Generic;
using System.Linq;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public static class GuildAccounts
    {
        //(auto-)creation of GuildAccounts
        private static GuildAccountStruct _guildAccount;
        private static string saveFile;

        public static void SaveAccount(ulong guildID)
        {
            saveFile = "GuildAccounts/" + guildID + ".json";
            GuildAccountsDataHandler.SaveGuildAccount(_guildAccount, saveFile);
        }

        public static GuildAccountStruct GetGuildAccount(ulong guildID)
        {
            string filePath = "GuildAccounts/" + guildID + ".json";
            var account = GuildAccountsDataHandler.GetGuildAccount(filePath);
            return account;
        }
    }
}