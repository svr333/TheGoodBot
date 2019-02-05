using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Core.Preconditions;
using TheGoodBot.Entities;

namespace TheGoodBot.Languages
{
    public class CustomEmbedService
    {
        private Dictionary<string, CustomEmbedStruct> _pairs;
        private string filePath = "Languages/LanguageFiles/English.json";
        private string directory = "Languages/LanguageFiles";

        public void CreateFile()
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(directory);
                var rawData = JsonConvert.SerializeObject(_pairs, Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }
        }

        private void SaveCustomEmbed(string key, CustomEmbedStruct embed)
        {
            var rawData = JsonConvert.SerializeObject(embed, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        public void AddOrChangePair(string key, CustomEmbedStruct embed)
        {
            if (!_pairs.ContainsKey(key)) { AddPair(key, embed); }
            else { ChangePair(key, embed); }
        }

        [RequireBotOwner]
        private void AddPair(string key, CustomEmbedStruct embed)
        {
            _pairs.Add(key, embed);
        }

        private void ChangePair(string key, CustomEmbedStruct embed)
        {
            var oldValue = _pairs[key];
            _pairs[key] = embed;
            var newValue = _pairs[key];
            CustomEmbedStruct[] editedValue = new CustomEmbedStruct[] {oldValue, newValue};
            SaveCustomEmbed(key, embed);
        }
    }
}