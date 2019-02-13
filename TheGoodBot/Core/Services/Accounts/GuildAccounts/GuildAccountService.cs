using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildAccountService
    {
        private List<ulong> guildIDs;
        private string filePath = "AllGuildID's.json";
        private CreateGuildAccountFilesService _guildFiles;

        public GuildAccountService(CreateGuildAccountFilesService guildFiles)
        {
            if (!File.Exists(filePath)) { CreateFile(); }
            string json = File.ReadAllText(filePath);
            guildIDs = JsonConvert.DeserializeObject<List<ulong>>(json);

            _guildFiles = guildFiles;
        }

        private void CreateFile()
        {
            File.WriteAllText(filePath, "");
        }

        public void CreateAllGuildAccounts()
        {
            guildIDs = new List<ulong>();
            for (int i = 0; i < guildIDs.Count; i++)
            {
                _guildFiles.CreateGuildAccount(guildIDs[i]);
            }
        }

        public void AddGuild(ulong ID)
        {
            guildIDs.Add(ID);
            SaveAccount(guildIDs);
        }

        private void SaveAccount(List<ulong> guildIDs)
        {
            var json = JsonConvert.SerializeObject(guildIDs, Formatting.Indented);
            File.WriteAllText(filePath, json);
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

        public void SaveGuildAccount(Settings guildAccount, ulong guildID)
        {
            string filePath = "GuildAccounts/" + guildID + ".json"; 
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public Settings GetSettingsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Settings.json";
            if (!File.Exists(filePath)) { _guildFiles.CreateGuildAccount(guildID);}
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Settings>(rawData);
        }

        public Cooldowns GetCooldownsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Cooldowns.json";
            if (!File.Exists(filePath)) { _guildFiles.CreateGuildAccount(guildID); }
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Cooldowns>(rawData);
        }

        public Stats GetStatsAccount(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}/Stats.json";
            if (!File.Exists(filePath)) { _guildFiles.CreateGuildAccount(guildID); }
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Stats>(rawData);
        }
    }
}