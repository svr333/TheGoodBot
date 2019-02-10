using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TheGoodOne.DataStorage
{
    public class CreateGuildAccountFilesService
    {
        private List<string> categoryList = new List<string>();
        private GuildFilesGenerationService _guildFilesGeneration;

        public CreateGuildAccountFilesService(GuildFilesGenerationService guildFilesGeneration)
        {
            _guildFilesGeneration = guildFilesGeneration;
        }

        public void CreateGuildAccount(ulong guildID)
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

                var rawData = JsonConvert.SerializeObject(categoryObject, Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }
        }

        private void GenerateNewCategoryList()
        {
            categoryList.Add("Settings");
            categoryList.Add("Cooldowns");
            categoryList.Add("Stats");
        }
    }
}