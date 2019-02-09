using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Languages
{
    public class CreateLanguageFilesService
    {
        private List<string> _languageList = new List<string>();

        private CommandService _commandService;

        public CreateLanguageFilesService(CommandService command)
        {
            _commandService = command;
        }

        public void CreateAllCommandFiles(string language)
        {
            var commandList = _commandService.Commands.ToList(); 
            string fileName = String.Empty;
            string directory = String.Empty;
            string filePath = String.Empty;

            for (int i = 0; i < commandList.Count; i++)
            {
                if (!(commandList[i].Module.Group == null))
                {
                    fileName = commandList[i].Module.Group + "-" + commandList[i].Name;
                }
                else  { fileName = commandList[i].Name; }

                directory = "Languages/" + language + "/" + commandList[i].Module.Name;
                filePath = directory + "/" + fileName + ".json";

                if (File.Exists(filePath)) { continue; }
                Directory.CreateDirectory(directory);

                var rawData = JsonConvert.SerializeObject(GenerateCustomEmbedStruct(), Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }          
        }

        public void CreateAllLanguageFiles()
        {
            if (_languageList == null || !_languageList.Any()) { GenerateNewLanguageList(); }

            foreach (var language in _languageList)
            {
                string filePath = "Languages/" + language;
                Directory.CreateDirectory(filePath);
                CreateAllCommandFiles(language);
            }
        }

        private CustomEmbedStruct GenerateCustomEmbedStruct() => new CustomEmbedStruct()
        {
            FieldTitles = null,
            FieldValues = null,
            FieldInlineValues = null,
            TimeStamp = DateTimeOffset.MinValue,
            Title = String.Empty,
            Description = String.Empty,
            AuthorName = String.Empty,
            FooterText = String.Empty,
            AuthorIconUrl = String.Empty,
            Colour = Color.Blue.RawValue,
            EmbedUrl = String.Empty,
            ThumbnailUrl = String.Empty,
            AuthorUrl = String.Empty,
            ImageUrl = String.Empty,
            FooterUrl = String.Empty,
            PlainText = String.Empty
        };

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