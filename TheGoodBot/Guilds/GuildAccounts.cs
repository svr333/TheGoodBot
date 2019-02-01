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

        public static void SaveAccount(GuildAccountStruct guildAccount, ulong guildID)
        {
            saveFile = "GuildAccounts/" + guildID + ".json";
            _guildAccount = guildAccount;
            GuildAccountService.SaveGuildAccount(_guildAccount, saveFile);
        }

        public static GuildAccountStruct GetGuildAccount(ulong guildID)
        {
            string filePath = "GuildAccounts/" + guildID + ".json";
            var account = GuildAccountService.GetGuildAccount(filePath);
            return account;
        }
    }
}