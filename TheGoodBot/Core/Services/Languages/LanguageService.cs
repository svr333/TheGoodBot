using TheGoodBot.Core.Services.Accounts;
using TheGoodBot.Core.Services.Accounts.GuildAccounts;
using TheGoodBot.Guilds;

namespace TheGoodBot.Core.Services.Languages
{
    public class LanguageService
    {
        private readonly GlobalUserAccountService _globalUserAccountService;
        private readonly GuildAccountService _guildAccountService;
        private readonly GuildUserAccountService _guildUserAccountService;

        public LanguageService( GuildAccountService guildAccount, GlobalUserAccountService globalUser, 
            GuildUserAccountService guildUser)
        {
            _guildAccountService = guildAccount;
            _globalUserAccountService = globalUser;
            _guildUserAccountService = guildUser;
        }

        public string GetLanguage(ulong guildId, ulong userId)
        {
            var guildAccount = _guildAccountService.GetSettingsAccount(guildId);
            var globalUser = _globalUserAccountService.GetOrCreateGlobalUserAccount(userId);
            var guildUser = _guildUserAccountService.GetOrCreateGuildUserAccount(guildId, userId);

            var language = string.Empty;

            if (guildAccount.AllowMembersOwnLanguageSetting == true)
            {
                if (globalUser.Language == string.Empty) { language = guildUser.Language; }
                else { language = globalUser.Language; }
            }
            else language = guildAccount.Language;

            if (string.IsNullOrEmpty(language)) { language = "English"; }

            return language;
        }

        
    }
}
