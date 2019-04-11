using Newtonsoft.Json;
using System;
using System.IO;
using TheGoodBot.Entities.GuildAccounts;

namespace TheGoodBot.Core.Services.Accounts.GuildAccounts
{
    public class GuildLogsService
    {
        private string filePath = $"";

        private void SetFilePath(ulong guildId)
            => filePath = $"GuildAccounts/{guildId}/GuildLogs.json";

        public GuildLogs GetGuildLogs(ulong guildId)
        {
            SetFilePath(guildId);
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<GuildLogs>(json);
        }

        public void SaveGuildLogs(GuildLogs log)
        {
            var json = JsonConvert.SerializeObject(log);
            File.WriteAllText(filePath, json);
        }

        public void AddToIgnoredUsersAndChannels()
        {
            throw new NotImplementedException();
        }

        public void RemoveFromIgnoredUsersAndChannels()
        {
            throw new NotImplementedException();
        }

        public void ChangeGuildLogs(ulong guildId, string guildLog, ulong newValue)
        {
            var currentLogs = GetGuildLogs(guildId);
            switch (guildLog)
            {
                default: return;
                case "MessageDeletedChannelId": currentLogs.MessageDeletedChannelId = newValue; break;
                case "MessageEditedChannelId": currentLogs.MessageEditedChannelId = newValue; break;
                case "PurgedMessagesChannelId": currentLogs.PurgedMessagesChannelId = newValue; break;

                case "UserJoinedChannelId": currentLogs.UserJoinedChannelId = newValue; break;
                case "UserLeftChannelId": currentLogs.UserLeftChannelId = newValue; break;
                case "UserKickedChannelId": currentLogs.UserKickedChannelId = newValue; break;
                case ".UserBannedChannelId": currentLogs.UserBannedChannelId = newValue; break;

                case "VoiceJoinedChannelId": currentLogs.VoiceJoinedChannelId = newValue; break;
                case "VoiceLeftChannelId": currentLogs.VoiceLeftChannelId = newValue; break;
                case "VoiceMovedChannelId": currentLogs.VoiceMovedChannelId = newValue; break;

                case "ChannelCreatedChannelId": currentLogs.ChannelCreatedChannelId = newValue; break;
                case "ChannelRemovedChannelId": currentLogs.ChannelRemovedChannelId = newValue; break;
                case "ChannelEditedChannelId": currentLogs.ChannelEditedChannelId = newValue; break;

                case "RoleCreatedChannelId": currentLogs.RoleCreatedChannelId = newValue; break;
                case "RoleDeletedChannelId": currentLogs.RoleDeletedChannelId = newValue; break;
                case "RoleEditedChannelId": currentLogs.RoleEditedChannelId = newValue; break;

                case "EmojiChannelId": currentLogs.EmojiChannelId = newValue; break;

                case "NameUpdatedChannelId": currentLogs.NameUpdatedChannelId = newValue; break;
                case "AvatarUpdatedChannelId": currentLogs.AvatarUpdatedChannelId = newValue; break;
            }
        }
    }
}
