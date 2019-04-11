using System.Collections.Generic;

namespace TheGoodBot.Entities.GuildAccounts
{
    public class GuildLogs
    {
        public ulong MessageDeletedChannelId { get; set; }
        public ulong MessageEditedChannelId { get; set; }
        public ulong PurgedMessagesChannelId { get; set; }

        public ulong UserJoinedChannelId { get; set; }
        public ulong UserLeftChannelId { get; set; }
        public ulong UserKickedChannelId { get; set; }
        public ulong UserBannedChannelId { get; set; }

        public ulong VoiceJoinedChannelId { get; set; }
        public ulong VoiceLeftChannelId { get; set; }
        public ulong VoiceMovedChannelId { get; set; }

        public ulong ChannelCreatedChannelId { get; set; }
        public ulong ChannelRemovedChannelId { get; set; }
        public ulong ChannelEditedChannelId { get; set; }

        public ulong RoleCreatedChannelId { get; set; }
        public ulong RoleDeletedChannelId { get; set; }
        public ulong RoleEditedChannelId { get; set; }

        public ulong EmojiChannelId { get; set; }

        public ulong NameUpdatedChannelId { get; set; }
        public ulong AvatarUpdatedChannelId { get; set; }

        public List<ulong> IgnoredUsersAndChannels { get; set; }
    }
}
