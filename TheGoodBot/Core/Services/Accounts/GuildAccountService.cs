using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildAccountService
    {
        private void CreateGuildAccount(ulong guildID)
        {
            string directory = "GuildAccounts";
            string saveFile = directory + "/" + guildID + ".json";

            if(CheckDirectoryExists(saveFile, directory)) return;

            string text = JsonConvert.SerializeObject(GenerateBlankGuildConfig(guildID), Formatting.Indented);
            File.WriteAllText(saveFile, text);
        }

        public GuildAccountStruct GetOrCreateGuildAccount(ulong guildID)
        {
            CreateGuildAccount(guildID);
            var guild = GetGuildAccount("GuildAccounts/" + guildID + ".json");
            return guild;
        }

        private bool CheckDirectoryExists(string filePath, string directory)
        {
            if (File.Exists(filePath)) { return true; }

            Directory.CreateDirectory(directory);
            return false;
        }

        public void SaveGuildAccount(GuildAccountStruct guildAccount, ulong guildID)
        {
            string filePath = "GuildAccounts/" + guildID + ".json"; 
            string rawData = JsonConvert.SerializeObject(guildAccount, Formatting.Indented);
            File.WriteAllText(filePath, rawData);
        }

        private GuildAccountStruct GetGuildAccount(string filePath)
        {
            string rawData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GuildAccountStruct>(rawData);
        }

        private GuildAccountStruct GenerateBlankGuildConfig(ulong guildID) => new GuildAccountStruct()
        {
            GuildID = guildID,
            ModRoles = null,
            PrefixList = new List<string>() {"!", "?"},
            AllMembersCombinedXP = 0,
            AllMembersCommandsExecuted = 0,
            AllMembersMessagesSent = 0,
            AllowMembersCustomEmbedColour = true,
            AllowMembersPrivateAccounts = true,
            NoCommandFoundIsDisabled = false,
            AllowedUsersOrRolesCheckPrivateAccounts = null
        };
    }
}