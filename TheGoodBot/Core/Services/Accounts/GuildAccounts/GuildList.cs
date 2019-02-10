using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TheGoodOne.DataStorage
{
    public class GuildList
    {
        private List<ulong> guildIDs;
        private string filePath = "AllGuildID's.json";
        private CreateGuildAccountFiles _createGuildAccountFiles;

        public GuildList(CreateGuildAccountFiles createGuildAccountFiles)
        {
            if (!File.Exists(filePath)) { CreateFile(); }
            _createGuildAccountFiles = createGuildAccountFiles;
            string json = File.ReadAllText(filePath);
            guildIDs = JsonConvert.DeserializeObject<List<ulong>>(json);
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
                _createGuildAccountFiles.CreateGuildAccount(guildIDs[i]);
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
    }
}