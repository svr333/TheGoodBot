using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Entities;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildAccountService
    {
        //checks whenever the bot joins a guild, if guild is already in database, bot will break out
        public static void CreateGuildAccount(ulong guildID)
        {
            string directory = "GuildAccounts";
            string saveFile = directory + "/" + guildID + ".json";
            //if the file exists, return
            if(CheckDirectoryExists(saveFile, directory)) return;


            //use json convert to write to .json file
            string text = JsonConvert.SerializeObject(GenerateBlankGuildConfig(), Formatting.Indented);
            File.WriteAllText(saveFile, text);
        }

        public GuildAccountStruct GetOrCreateAccount(ulong guildID)
        {
            CreateGuildAccount(guildID);
            var guild = GetGuildAccount("GuildAccounts/" + guildID + ".json");
            return guild;
        }

        public static bool CheckDirectoryExists(string filePath, string directory)
        {
            if (File.Exists(filePath)) { return true; }

            Directory.CreateDirectory(directory);
            return false;
        }

        //can be called to save guild accounts
        public static void SaveGuildAccount(GuildAccountStruct guildAccount, string filePath)
        {
            //save guild account
            if (!SaveExists(filePath)) return;
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        //returns guild account
        public static GuildAccountStruct GetGuildAccount(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GuildAccountStruct>(rawData);
        }

        public static bool SaveExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static GuildAccountStruct GenerateBlankGuildConfig() => new GuildAccountStruct()
        {
            modRoles = null,
            prefixesList = new List<string>() {"!", "?"},
            guildID = 0,
            allMembersCombinedXP = 0,
            allMembersCommandsExecuted = 0,
            allMembersMessagesSent = 0,
            allowMembersCustomEmbedColour = true,
            allowMembersPrivateAccounts = true
                
        };
    }
}