using System.Collections.Generic;
using TheGoodBot.Entities.GuildAccounts;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class GuildFilesGenerationService
    {

        public object GetAndCreateObject(string category, ulong guildID)
        {
            if (category == "Settings") { return GenerateBlankSettingsFile(guildID); }
            if (category == "Stats") { return GenerateBlankStatsFile(); }
            if (category == "GuildLogs") { return GenerateBlankGuildLogsFile(); }
            else { return null; }
        }

        private Stats GenerateBlankStatsFile() => new Stats()
        {
            AllMembersCombinedXp = 0,
            AllMembersCommandsExecuted = 0,
            AllMembersMessagesSent = 0
        };

        private Settings GenerateBlankSettingsFile(ulong guildId) => new Settings()
        {
            GuildID = guildId,
            ModRoles = null,
            PrefixList = new List<string>() { "!", "?" },
            AllowMembersCustomEmbedColour = true,
            AllowMembersPrivateAccounts = true,
            NoCommandFoundResponseIsDisabled = false,
            AllowedUsersOrRolesCheckPrivateAccounts = null,
            Language = "English",
            AllowMembersOwnLanguageSetting = true,
            BotsCanInteract = true,
            AllowAdminsToBypassCooldowns = true,
            AllowedUsersAndRolesToBypassCooldowns = new List<ulong>(),
            GlobalCooldown = 0,
            GlobalInvocationTime = 0,
            ResetEXPOnBan = false,
            ResetEXPOnLeave = false,
            PurgePinnedMessages = false
        };

        private GuildLogs GenerateBlankGuildLogsFile() => new GuildLogs()
        {
            IgnoredUsersAndChannels = new List<ulong> { 202095042372829184 },
            AvatarUpdatedChannelId = 0,
            ChannelCreatedChannelId = 0,
            ChannelEditedChannelId = 0,
            ChannelRemovedChannelId = 0,
            EmojiChannelId = 0,
            MessageDeletedChannelId = 0,
            MessageEditedChannelId = 0,
            NameUpdatedChannelId = 0,
            PurgedMessagesChannelId = 0,
            RoleCreatedChannelId = 0,
            RoleDeletedChannelId = 0,
            RoleEditedChannelId = 0,
            UserBannedChannelId = 0,
            UserJoinedChannelId = 0,
            UserKickedChannelId = 0,
            UserLeftChannelId = 0,
            VoiceJoinedChannelId = 0,
            VoiceLeftChannelId = 0,
            VoiceMovedChannelId = 0
        };
    }
}
