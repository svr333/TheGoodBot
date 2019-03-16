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
        private List<string> _unchangeableEmbedList = new List<string>();

        private CommandService _commandService;

        public CreateLanguageFilesService(CommandService command)
        {
            _commandService = command;
        }

        /// <summary> Creates the language's corresponding embed files.</summary>
        /// <param name="language"></param>
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
                    fileName = commandList[i].Module.Group.ToLower() + "-" + commandList[i].Name.ToLower();
                }
                else  { fileName = commandList[i].Name.ToLower(); }

                directory = $"Languages/{language}/{commandList[i].Module.Name.ToLower()}";
                filePath = $"{directory}/{fileName}.json";

                if (File.Exists(filePath)) { continue; }
                Directory.CreateDirectory(directory);

                var rawData = JsonConvert.SerializeObject(GenerateCustomEmbedStruct(), Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }
        }
        /// <summary>Creates all the required embeds for static core functionality IF they don't already exist.</summary>
        /// <param name="language"></param>
        public void CreateAllUnchangeableEmbeds(string language)
        {
            foreach (var embed in _unchangeableEmbedList)
            {
                string filePath = $"Languages/{language}/!UnchangeableEmbeds/{embed}.json";

                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory($"Languages/{language}/!UnchangeableEmbeds");
                    var rawData = JsonConvert.SerializeObject(GenerateCustomEmbedStruct(), Formatting.Indented);
                    File.WriteAllText(filePath, rawData);
                }
            }
        }

        /// <summary> Creates all the languages and the corresponding files.</summary>
        public void CreateAllLanguageFiles()
        {
            if (_languageList == null || !_languageList.Any()) { GenerateNewLanguageList(); }
            if (_unchangeableEmbedList == null || !_unchangeableEmbedList.Any()) { CreateUnchangeableEmbedList(); }

            foreach (var language in _languageList)
            {
                string filePath = $"Languages/{language}";
                Directory.CreateDirectory(filePath);
                CreateAllCommandFiles(language);
                CreateAllUnchangeableEmbeds(language);
            }
        }

        /// <summary> Generates the custom embed structure for the json files.</summary>
        private CustomEmbed GenerateCustomEmbedStruct() => new CustomEmbed()
        {
            FieldTitles = null,
            FieldValues = null,
            FieldInlineValues = null,
            TimeStamp = null,
            Title = String.Empty,
            Description = String.Empty,
            AuthorName = String.Empty,
            FooterText = String.Empty,
            AuthorIconUrl = String.Empty,
            Colour = "000000",
            EmbedUrl = String.Empty,
            ThumbnailUrl = String.Empty,
            AuthorUrl = String.Empty,
            ImageUrl = String.Empty,
            FooterUrl = String.Empty,
            PlainText = String.Empty
        };

        /// <summary> Creates the list of all languages.</summary>
        private void GenerateNewLanguageList()
        {
            _languageList.Clear();
            _languageList.Add("English");
            _languageList.Add("Dutch");
            _languageList.Add("French");
            _languageList.Add("Spanish");
        }

        /// <summary> Creates the list of unchangeable embeds.</summary>
        private void CreateUnchangeableEmbedList()
        {
            _unchangeableEmbedList.Clear();
            _unchangeableEmbedList.Add("NoCommandFound");
            _unchangeableEmbedList.Add("UncalculatedError");
            _unchangeableEmbedList.Add("CommandOnCooldown");
            _unchangeableEmbedList.Add("NoValidPermissions");
            _unchangeableEmbedList.Add("NoBotOwner");
            _unchangeableEmbedList.Add("FieldFailure");
            _unchangeableEmbedList.Add("RequireNSFW");
        }
    }
}