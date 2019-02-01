using System.Collections.Generic;

namespace TheGoodBot.Guilds
{
    public class GuildAccountStruct
    {
        //list of settings
        List<string> prefixesList = new List<string>(){"?", "!"};
        public bool allowMembersCustomEmbedColour { get; set; }

        //list of leaderboard stats
        public uint allMembersCombinedXP { get; set; }
        public uint allMembersCommandsExecuted { get; set; }
        public uint allMembersMessagesSent { get; set; }

        /* MUSIC COMMANDS (implement later)
         * private uint allMembersSongsQueued { get; set; }
         * private uint allMembersHoursMusicPlayed { get; set; }
        */
    }
}