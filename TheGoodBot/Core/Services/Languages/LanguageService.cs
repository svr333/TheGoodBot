using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord.Commands;
using TheGoodBot.Languages;

namespace TheGoodBot.Entities
{
    public class LanguageService
    {
        private List<string> _languageList = new List<string>();
        private ChangeCustomEmbedService _changeCustomEmbedService;

        public LanguageService(ChangeCustomEmbedService changeCustomEmbedService = null)
        {
            _changeCustomEmbedService = changeCustomEmbedService;
        }

        public void CreateLanguageFiles()
        {
            //if (_languageList == null || !_languageList.Any()) { GenerateNewLanguageList(); }
            GenerateNewLanguageList();

            foreach (var language in _languageList)
            {
                string filePath = "Languages/" + language;
                Directory.CreateDirectory(filePath);
                _changeCustomEmbedService.CreateAllCommandFiles(language);
            }
        }

        public void GenerateNewLanguageList()
        {
            _languageList.Clear();
            _languageList.Add("English");
            _languageList.Add("Dutch");
            _languageList.Add("French");
            _languageList.Add("Spanish");
        }
    }
}