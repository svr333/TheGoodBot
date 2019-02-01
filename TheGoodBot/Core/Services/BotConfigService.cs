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
        /// <summary>
        /// Gets the information from the Config.Json file for you to use in the bot.
        /// </summary>
        /// <returns></returns>
        public BotConfig GetConfig()
            => GetBotConfigData();

        /// <summary>
        /// Private function to hide the implementation of this method.
        /// </summary>
        /// <returns></returns>
        private BotConfig GetBotConfigData()
        {
            CheckConfigExists();
            var rawData = File.ReadAllText(ConfigLocation);
            return JsonConvert.DeserializeObject<BotConfig>(rawData);
        }

        /// <summary>
        /// Checks if the Config.Json exists, if it doesn't it creates one for you to fill out.
        /// </summary>
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

        /// <summary>
        /// Generates a basic Config.Json for you to fill out with your info.
        /// </summary>
        /// <returns></returns>
        private BotConfig GenBlankConfig()
            => new BotConfig
            {
                discordToken = "CHANGE ME TO YOUR DISCORD TOKEN",
                gameStatus = "CHANGE ME TO WHATEVER GAME STATUS YOU WANT TO DISPLAY",
                botOwnerID = 202095042372829184
            };
    }
}
