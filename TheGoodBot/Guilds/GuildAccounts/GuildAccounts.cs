using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Guilds
{
    public class GuildAccounts
    {
        //(auto-)creation of GuildAccounts
        private static List<GuildAccountStruct> _guildAccount;
        private static string saveFile;

        public void SaveAccounts()
        {
            GuildAccountsDataHandler.SaveGuildAccount(_guildAccount, saveFile);
        }
    }
}