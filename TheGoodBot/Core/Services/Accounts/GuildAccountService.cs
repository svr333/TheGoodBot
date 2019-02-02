using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildAccountService
    {
        //checks whenever the bot joins a guild, if guild is already in database, bot will break out
        public void CreateGuildAccount(ulong guildID)
        {
            string directory = "GuildAccounts";
            string saveFile = directory + "/" + guildID + ".json";
            //if the file exists, return
            if(CheckDirectoryExists(saveFile, directory)) return;
            //use json convert to write to .json file
            string text = JsonConvert.SerializeObject(GenerateBlankGuildConfig(guildID), Formatting.Indented);
            File.WriteAllText(saveFile, text);
        }

        //gets a guild account if exists, otherwise creates one and then gets it
        public GuildAccountStruct GetOrCreateGuildAccount(ulong guildID)
        {
            CreateGuildAccount(guildID);
            var guild = GetGuildAccount("GuildAccounts/" + guildID + ".json");
            return guild;
        }

        public bool CheckDirectoryExists(string filePath, string directory)
        {
            if (File.Exists(filePath)) { return true; }

            Directory.CreateDirectory(directory);
            return false;
        }

        //can be called to save guild accounts
        public void SaveGuildAccount(GuildAccountStruct guildAccount, ulong guildID)
        {
            //save guild account
            string filePath = "GuildAccounts/" + guildID + ".json"; 
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        //returns guild account
        public GuildAccountStruct GetGuildAccount(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GuildAccountStruct>(rawData);
        }

        public GuildAccountStruct GenerateBlankGuildConfig(ulong guildID) => new GuildAccountStruct()
        {
            guildID = guildID,
            modRoles = null,
            prefixesList = new List<string>() {"!", "?"},
            allMembersCombinedXP = 0,
            allMembersCommandsExecuted = 0,
            allMembersMessagesSent = 0,
            allowMembersCustomEmbedColour = true,
            allowMembersPrivateAccounts = true
                
        };
    }
}