using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildAccountService
    {
        private CreateGuildAccountFilesService _guildFiles;

        public GuildAccountService(CreateGuildAccountFilesService guildFiles)
        {
            _guildFiles = guildFiles;
        }

        private void CreateGuildAccount(ulong guildID)
        {
            _guildFiles.CreateGuildAccount(guildID);
        }

        public GuildAccountStruct GetOrCreateGuildAccountCategory(ulong guildID, string category)
        {
            CreateGuildAccount(guildID);
            var guild = GetGuildAccount($"GuildAccounts/{guildID}/{category}.json", category);
            return guild as GuildAccountStruct;
        }

        private bool CheckDirectoryExists(string filePath, string directory)
        {
            if (File.Exists(filePath)) { return true; }

            Directory.CreateDirectory(directory);
            return false;
        }

        public void SaveGuildAccount(GuildAccountStruct guildAccount, ulong guildID)
        {
            string filePath = "GuildAccounts/" + guildID + ".json"; 
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        private object GetGuildAccount(string filePath, string category)
        {
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<object>(rawData);
        }
    }
}