using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TheGoodBot.Guilds
{
    public class CreateGuildAccountFilesService
    {
        private List<string> categoryList = new List<string>();
        private GuildFilesGenerationService _guildFilesGeneration;
        private CooldownService _cooldown;

        public CreateGuildAccountFilesService(GuildFilesGenerationService guildFilesGeneration, CooldownService cooldown) 
        {
            _guildFilesGeneration = guildFilesGeneration;
            _cooldown = cooldown;
        }

        public void CreateGuildAccountFiles(ulong guildID)
        {
            string filePath = $"GuildAccounts/{guildID}";
            Directory.CreateDirectory(filePath);
            CreateGuildFiles(guildID);
        }

        private void CreateGuildFiles(ulong guildID)
        {
            if (categoryList == null || !categoryList.Any()) { GenerateNewCategoryList(); }

            for (int i = 0; i < categoryList.Count; i++)
            {
                string filePath = $"GuildAccounts/{guildID}/{categoryList[i]}.json";
                if (File.Exists(filePath)) { continue; }

                var categoryObject = _guildFilesGeneration.GetAndCreateObject(categoryList[i], guildID);
                _cooldown.CreateAllPairs(guildID);

                var rawData = JsonConvert.SerializeObject(categoryObject, Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }
        }

        private void GenerateNewCategoryList()
        {
            categoryList.Add("Settings");
            categoryList.Add("Stats");
            categoryList.Add("GuildLogs");
        }
    }
}