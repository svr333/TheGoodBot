using System;
using System.IO;
using System.Linq;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using TheGoodBot.Entities;

namespace TheGoodBot.Languages
{
    public class ChangeCustomEmbedService
    {
        private CommandService _commandService;

        public ChangeCustomEmbedService(CommandService command)
        {
            _commandService = command;
        }

        public void ValidateAndCreateFiles()
        {
            var commandList = _commandService.Commands.ToList(); 
            string fileName = String.Empty;
            string directory = String.Empty;
            string filePath = String.Empty;

            for (int i = 0; i < commandList.Count; i++)
            {
                if (!(commandList[i].Module.Group == String.Empty))
                {
                    fileName = commandList[i].Module.Group + "-" + commandList[i].Name;
                }
                else  { fileName = commandList[i].Name; }

                directory = commandList[i].Module.Name;
                filePath = directory + "/" + fileName + ".json";

                if (File.Exists(filePath)) { return; }
                var embed = GenerateCustomEmbedStruct();
                Directory.CreateDirectory(directory);

                var rawData = JsonConvert.SerializeObject(embed, Formatting.Indented);
                File.WriteAllText(filePath, rawData);
            }          
        }

        public void ChangeCustomEmbed()
        {

        }

        public CustomEmbedStruct GenerateCustomEmbedStruct() => new CustomEmbedStruct()
        {
            FieldTitles = null,
            FieldValues = null,
            FieldInlineValues = null,
            TimeStamp = DateTimeOffset.UtcNow,
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
    }
}