using System.Collections.Generic;
using System.Dynamic;
using Discord.WebSocket;

namespace TheGoodBot.Guilds
{
    public class GuildAccountStruct
    {
        //list of settings
        public ulong guildID { get; set; }
        public List<string> prefixesList { get; set; }
        public List<SocketRole> modRoles { get; set; }
        public bool allowMembersCustomEmbedColour { get; set; }
        public bool allowMembersPrivateAccounts { get; set; }
        public List<ulong> allowedUsersOrRolesCheckPrivateAccounts { get; set; }

        //list of leaderboard stats
        public uint allMembersCombinedXP { get; set; }
        public uint allMembersCommandsExecuted { get; set; }
        public uint allMembersMessagesSent { get; set; }

        public struct Music
        {
            private uint allMembersSongsQueued { get; set; }
            private uint allMembersHoursMusicPlayed { get; set; }
        }
    }
}