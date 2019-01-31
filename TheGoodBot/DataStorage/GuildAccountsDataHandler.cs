using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public static class GuildAccountsDataHandler
    {
        //can be called to save guild accounts
        public static void SaveGuildAccounts(GuildAccountStruct guildAccount, string filePath)
        {
            //save guild account
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        //Get or creates guild account on first use
        public static IEnumerable<GuildAccountStruct> GetOrCreateGuildAccount(string filePath, GuildAccountStruct guildAccount)
        {
            //FILEPATH NEEDS TO BE "GUILDACCOUNTS/CONTEXT.GUILD.ID"
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                string text = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
                File.WriteAllText(filePath, text);
            }

            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<GuildAccountStruct>>(rawData);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}