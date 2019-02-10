using System;
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

        public GuildAccountStruct GetSettingsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Settings.json";
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GuildAccountStruct>(rawData);
        }

        public CooldownsStruct GetCooldownsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<CooldownsStruct>(rawData);
        }

        public StatsStruct GetStatsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Stats.json";
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<StatsStruct>(rawData);
        }
    }
}