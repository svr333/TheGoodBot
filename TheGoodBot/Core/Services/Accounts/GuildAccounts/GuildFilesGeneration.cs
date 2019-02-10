using System.Collections.Generic;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildFilesGeneration
    {
        public object GetAndCreateObject(string category, ulong guildID)
        {
            if (category == "Settings") { return GenerateBlankSettingsFile(guildID); }
            if (category == "Cooldowns") { return GenerateBlankCooldownsFile(); }
            else { return null; }
        }

        private GuildAccountStruct GenerateBlankSettingsFile(ulong guildID) => new GuildAccountStruct()
        {
            GuildID = guildID,
            ModRoles = null,
            PrefixList = new List<string>() { "!", "?" },
            AllMembersCombinedXP = 0,
            AllMembersCommandsExecuted = 0,
            AllMembersMessagesSent = 0,
            AllowMembersCustomEmbedColour = true,
            AllowMembersPrivateAccounts = true,
            NoCommandFoundIsDisabled = false,
            AllowedUsersOrRolesCheckPrivateAccounts = null,
            Language = "English",
            AllowMembersOwnLanguageSetting = true
        };

        private CooldownsStruct GenerateBlankCooldownsFile() => new CooldownsStruct()
        {
            Guild = 0,
            Test = 0
        };
    }
}