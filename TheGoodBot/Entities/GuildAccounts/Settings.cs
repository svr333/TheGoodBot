using System.Collections.Generic;
using System.Dynamic;
using Discord.WebSocket;

namespace TheGoodBot.Guilds
{
    public class Settings
    {
        //list of settings
        public ulong GuildID { get; set; }
        public List<string> PrefixList { get; set; }
        public List<ulong> ModRoles { get; set; }
        public string Language { get; set; }
        public bool NoCommandFoundIsDisabled { get; set; }
        public bool AllowMembersCustomEmbedColour { get; set; }
        public bool AllowMembersPrivateAccounts { get; set; }
        public bool AllowMembersOwnLanguageSetting { get; set; }
        public List<ulong> AllowedUsersOrRolesCheckPrivateAccounts { get; set; }
    }
}