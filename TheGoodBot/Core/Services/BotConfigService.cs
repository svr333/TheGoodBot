using TheGoodBot.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace TheGoodOne.DataStorage
{
    public class BotConfigService
    {
        private readonly string ConfigLocation = "config.json";
        public BotConfig GetConfig()
            => GetBotConfigData();

        private BotConfig GetBotConfigData()
        {
            CheckConfigExists();
            var rawData = File.ReadAllText(ConfigLocation);
            return JsonConvert.DeserializeObject<BotConfig>(rawData);
        }

        private void CheckConfigExists()
        {
            if (!File.Exists(ConfigLocation))
            {
                Console.WriteLine($"No Config Was Found " +
                    $"\nA New one has been generated at: {Directory.GetCurrentDirectory()} " +
                    $"\nPlease fill out the values and restart the bot.");
                var json = JsonConvert.SerializeObject(GenBlankConfig(), Formatting.Indented);
                File.WriteAllText(ConfigLocation, json, Encoding.UTF8);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        private BotConfig GenBlankConfig()
            => new BotConfig
            {
                DiscordToken = "CHANGE ME TO YOUR DISCORD TOKEN",
                GameStatus = "CHANGE ME TO WHATEVER GAME STATUS YOU WANT TO DISPLAY",
                BotOwnerID = 202095042372829184
            };
    }
}
