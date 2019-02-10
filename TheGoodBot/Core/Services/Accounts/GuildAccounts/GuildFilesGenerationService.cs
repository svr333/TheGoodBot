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
            if (category == "Cooldowns") { return GenerateBlankCooldownsFile(); }
            if (category == "Stats") { return GenerateBlankStatsFile(); }
            else { return null; }
        }

        private object GenerateBlankStatsFile() => new StatsStruct()
        {
            AllMembersCombinedXP = 0,
            AllMembersCommandsExecuted = 0,
            AllMembersMessagesSent = 0
        };

        private GuildAccountStruct GenerateBlankSettingsFile(ulong guildID) => new GuildAccountStruct()
        {
            GuildID = guildID,
            ModRoles = null,
            PrefixList = new List<string>() { "!", "?" },
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