using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
     public static class GuildAccountsDataHandler
    {
        
        //checks whenever the bot joins a guild, if guild is already in database, bot will break out
        public static void CreateGuildAccount(ulong guildID)
        {
            string directory = "GuildAccounts/";
            string saveFile = directory + guildID + ".json";
            List<GuildAccountStruct> guildAccount;

            if (!SaveExists(saveFile))
            {
                guildAccount = new List<GuildAccountStruct>();
                File.Create(saveFile);
            }
            else return;
            string text = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(saveFile, text);
        }

        //can be called to save guild accounts
        public static void SaveGuildAccount(IEnumerable<GuildAccountStruct> guildAccount, string filePath)
        {
            //save guild account
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        //returns guild account
        public static IEnumerable<GuildAccountStruct> GetGuildAccount(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuildAccountStruct>>(rawData);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}