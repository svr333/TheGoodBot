using System.Collections.Generic;

namespace TheGoodBot.Guilds
{
    public class GuildAccountStruct
    {
        //list of settings
        List<string> prefixesList = new List<string>(){"?", "!"};
        private bool allowMembersCustomEmbedColour { get; set; }

        //list of leaderboard stats
        private uint allMembersCombinedXP { get; set; }
        private uint allMembersCommandsExecuted { get; set; }
        private uint allMembersMessagesSent { get; set; }

        /* MUSIC COMMANDS (implement later)
         * private uint allMembersSongsQueued { get; set; }
         * private uint allMembersHoursMusicPlayed { get; set; }
        */
    }
}