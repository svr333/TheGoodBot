using System;
using System.Collections.Generic;
using TheGoodBot.Guilds;

namespace TheGoodOne.DataStorage
{
    public class GuildFilesGenerationService
    {

        public object GetAndCreateObject(string category, ulong guildID)
        {
            if (category == "Settings") { return GenerateBlankSettingsFile(guildID); }
            if (category == "Stats") { return GenerateBlankStatsFile(); }
            else { return null; }
        }

        private object GenerateBlankStatsFile() => new Stats()
        {
            AllMembersCombinedXP = 0,
            AllMembersCommandsExecuted = 0,
            AllMembersMessagesSent = 0
        };

        private Settings GenerateBlankSettingsFile(ulong guildID) => new Settings()
        {
            GuildID = guildID,
            ModRoles = null,
            PrefixList = new List<string>() { "!", "?" },
            AllowMembersCustomEmbedColour = true,
            AllowMembersPrivateAccounts = true,
            NoCommandFoundResponseIsDisabled = false,
            AllowedUsersOrRolesCheckPrivateAccounts = null,
            Language = "English",
            AllowMembersOwnLanguageSetting = true
        };
    }
}