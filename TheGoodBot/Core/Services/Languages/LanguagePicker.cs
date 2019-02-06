using System;
using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Guilds;
using TheGoodOne.DataStorage;

namespace TheGoodBot.Languages
{
    public class LanguagePicker
    {
        private GlobalUserAccountService _globalUserAccountService;
        private GuildAccountService _guildAccountService;
        private GuildUserAccountService _guildUserAccountService;

        public LanguagePicker(GuildAccountService GuildAccount = null, GlobalUserAccountService GlobalUser = null, GuildUserAccountService GuildUser = null)
        {
            _guildAccountService = GuildAccount;
            _globalUserAccountService = GlobalUser;
            _guildUserAccountService = GuildUser;

        }

        public string GetLanguage(ulong guildID, ulong userID)
        {
            var guildAccount = _guildAccountService.GetOrCreateGuildAccount(guildID);
            var globalUser = _globalUserAccountService.GetOrCreateGlobalUserAccount(userID);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(guildID, userID);


            if (guildAccount.AllowMembersOwnLanguageSetting == true)
            {
                if (globalUser.Language == String.Empty) { return guildUser.Language; }
                else { return globalUser.Language; }
            }
            return guildAccount.Language;
        }
    }
}